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

using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.IO;
using System.Text;
using Tuvi.Auth.Proton;
using Tuvi.Proton.Primitive.Exceptions;

namespace Tuvi.Proton.Primitive.Modules
{
    public class StandardPGPModule : IPGPModule
    {
        private string modulusPubkey = Constants.SRP_MODULUS_KEY;

        public void ImportKeys(string keyData)
        {
            modulusPubkey = keyData ?? throw new ArgumentNullException(nameof(keyData));
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
                    UpdateSignature(sig, bOut.ToArray());
                    return sig.Verify();
                }
            }
        }

        internal static string GetModulusData(string message)
        {
            using (ArmoredInputStream aIn = GetArmoredInputStream(message))
            using (MemoryStream bOut = GetClearTextStream(aIn))
            {
                return Encoding.UTF8.GetString(bOut.ToArray()).Trim();
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

        private static void UpdateSignature(
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

        private static void Fail(
            string message)
        {
            throw new WrongHeaderException(message);
        }
    }
}
