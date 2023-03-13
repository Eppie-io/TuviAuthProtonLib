////////////////////////////////////////////////////////////////////////////////
//
//   Copyright 2023 Eppie(https://eppie.io)
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
////////////////////////////////////////////////////////////////////////////////

using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Bcpg;
using System;
using System.IO;
using System.Text;
using Tuvi.Proton.Primitive.Exceptions;
using Tuvi.Auth.Proton;

namespace Tuvi.Proton.Primitive.Modules
{
    public class StandardPGPModule : IPGPModule
    {
        private string modulusPubkey = Constants.SRP_MODULUS_KEY;

        public void ImportKeys(string keyData)
        {
            if (keyData == null)
            {
                throw new ArgumentNullException(nameof(keyData));
            }

            modulusPubkey = keyData;
        }

        public string ReadSignedMessage(string message)
        {
            if (VerifyMessageSignature(message))
            {
                return GetModulusData(message);
            }
            
            throw new ArgumentException("The message signature is not correct.", nameof(message));
        }

        internal bool VerifyMessageSignature(string message)
        {
            using (ArmoredInputStream aIn = GetArmoredInputStream(message))                 
            {
                using (MemoryStream bOut = GetClearTextStream(aIn))
                {
                    PgpPublicKeyRingBundle pgpRings;

                    using (ArmoredInputStream keyIn = new ArmoredInputStream(
                        new MemoryStream(Encoding.ASCII.GetBytes(modulusPubkey))))
                    {
                        pgpRings = new PgpPublicKeyRingBundle(keyIn);
                    }

                    PgpObjectFactory pgpFact = new PgpObjectFactory(aIn);
                    PgpSignatureList p3 = (PgpSignatureList)pgpFact.NextPgpObject();
                    PgpSignature sig = p3[0];

                    sig.InitVerify(pgpRings.GetPublicKey(sig.KeyId));
                    UpdateSignature(bOut, sig);
                    return sig.Verify();
                }
            }
        }

        internal static string GetModulusData(string message)
        {
            using (ArmoredInputStream aIn = GetArmoredInputStream(message))
            using (MemoryStream bOut = GetClearTextStream(aIn))
            {
                var sB = new StringBuilder();
                ProcessClearText(bOut, (line, isFirst) =>
                {
                    sB.Append(Encoding.UTF8.GetString(line.ToArray()).Trim());
                });
                //sB.Replace("\r", "");
                //sB.Replace("\n", "");
                return sB.ToString().Trim();
            }
        }

        private static ArmoredInputStream GetArmoredInputStream(string message)
        {
            ArmoredInputStream aIn = new ArmoredInputStream(
                new MemoryStream(Encoding.ASCII.GetBytes(message)));

            try
            {
                string[] headers = aIn.GetArmorHeaders();

                if (headers == null || headers.Length != 1)
                {
                    Fail("wrong number of headers found");
                }

                if (!"Hash: SHA256".Equals(headers[0], StringComparison.Ordinal))
                {
                    Fail("header value wrong: " + headers[0]);
                }
            }
            catch
            {
                aIn.Dispose();
                throw;
            }

            return aIn;
        }

        private static MemoryStream GetClearTextStream(ArmoredInputStream aIn)
        {
            MemoryStream bOut = new MemoryStream();
            int ch;

            try
            {
                while ((ch = aIn.ReadByte()) >= 0 && aIn.IsClearText())
                {
                    bOut.WriteByte((byte)ch);
                }
            }
            catch
            {
                bOut.Dispose();
                throw;
            }

            return bOut;
        }

        private static void UpdateSignature(MemoryStream bOut, PgpSignature sig)
        {
            ProcessClearText(bOut, (line, isFirst) =>
            {
                if (!isFirst)
                {
                    sig.Update((byte)'\r');
                    sig.Update((byte)'\n');
                }
                ProcessLine(sig, line.ToArray());
            });
        }

        private static void ProcessClearText(MemoryStream bOut, Action<MemoryStream, bool> processLine)
        {
            using (MemoryStream lineOut = new MemoryStream())
            {
                using (Stream sigIn = new MemoryStream(bOut.ToArray(), false))
                {
                    int lookAhead = ReadInputLine(lineOut, sigIn);

                    processLine(lineOut, true);

                    if (lookAhead != -1)
                    {
                        do
                        {
                            lookAhead = ReadInputLine(lineOut, lookAhead, sigIn);

                            processLine(lineOut, false);
                        }
                        while (lookAhead != -1);
                    }
                }
            }
        }

        private static int ReadInputLine(
            MemoryStream bOut,
            Stream fIn)
        {
            bOut.SetLength(0);

            int lookAhead = -1;
            int ch;

            while ((ch = fIn.ReadByte()) >= 0)
            {
                bOut.WriteByte((byte)ch);
                if (ch == '\r' || ch == '\n')
                {
                    lookAhead = ReadPassedEol(bOut, ch, fIn);
                    break;
                }
            }

            return lookAhead;
        }

        private static int ReadInputLine(
            MemoryStream bOut,
            int lookAhead,
            Stream fIn)
        {
            bOut.SetLength(0);

            int ch = lookAhead;

            do
            {
                bOut.WriteByte((byte)ch);
                if (ch == '\r' || ch == '\n')
                {
                    lookAhead = ReadPassedEol(bOut, ch, fIn);
                    break;
                }
            }
            while ((ch = fIn.ReadByte()) >= 0);

            return lookAhead;
        }

        private static int ReadPassedEol(
            MemoryStream bOut,
            int lastCh,
            Stream fIn)
        {
            int lookAhead = fIn.ReadByte();

            if (lastCh == '\r' && lookAhead == '\n')
            {
                bOut.WriteByte((byte)lookAhead);
                lookAhead = fIn.ReadByte();
            }

            return lookAhead;
        }

        internal static void Fail(
            string message)
        {
            throw new WrongHeaderException(message);
        }

        private static void ProcessLine(
            PgpSignature sig,
            byte[] line)
        {
            int length = GetLengthWithoutWhiteSpace(line);
            if (length > 0)
            {
                sig.Update(line, 0, length);
            }
        }

        private static int GetLengthWithoutWhiteSpace(
            byte[] line)
        {
            int end = line.Length - 1;

            while (end >= 0 && IsWhiteSpace(line[end]))
            {
                end--;
            }

            return end + 1;
        }

        private static bool IsWhiteSpace(
            byte b)
        {
            return b == '\r' || b == '\n' || b == '\t' || b == ' ';
        }
    }
}
