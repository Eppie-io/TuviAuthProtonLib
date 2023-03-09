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
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tuvi.Proton.Primitive.Data;
using Tuvi.Proton.Primitive.Exceptions;

namespace Tuvi.Proton.Primitive.Modules
{
    public class StandardPGPModule : IPGPModule
    {
        private const string modulusPubkey = "-----BEGIN PGP PUBLIC KEY BLOCK-----\r\n\r\nxjMEXAHLgxYJKwYBBAHaRw8BAQdAFurWXXwjTemqjD7CXjXVyKf0of7n9Ctm\r\nL8v9enkzggHNEnByb3RvbkBzcnAubW9kdWx1c8J3BBAWCgApBQJcAcuDBgsJ\r\nBwgDAgkQNQWFxOlRjyYEFQgKAgMWAgECGQECGwMCHgEAAPGRAP9sauJsW12U\r\nMnTQUZpsbJb53d0Wv55mZIIiJL2XulpWPQD/V6NglBd96lZKBmInSXX/kXat\r\nSv+y0io+LR8i2+jV+AbOOARcAcuDEgorBgEEAZdVAQUBAQdAeJHUz1c9+KfE\r\nkSIgcBRE3WuXC4oj5a2/U3oASExGDW4DAQgHwmEEGBYIABMFAlwBy4MJEDUF\r\nhcTpUY8mAhsMAAD/XQD8DxNI6E78meodQI+wLsrKLeHn32iLvUqJbVDhfWSU\r\nWO4BAMcm1u02t4VKw++ttECPt+HUgPUq5pqQWe5Q2cW4TMsE\r\n=Y4Mw\r\n-----END PGP PUBLIC KEY BLOCK-----";

        public void ImportKeys(string keyData)
        {
            throw new NotImplementedException();
        }

        public IVerifiedModulus VerifyModulus(string modulus)
        {
            bool isValid = MessageSignatureChecking(modulus);
            string data = GetModulusData(modulus);
            IVerifiedModulus verifiedModulus = new StandardVerifiedModulus() { Data = data, IsValid = isValid };
            return verifiedModulus;
        }

        internal static bool MessageSignatureChecking(string message)
        {
            using (ArmoredInputStream aIn = new ArmoredInputStream(
                new MemoryStream(Encoding.ASCII.GetBytes(message))))
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

                //
                // read the input, making sure we ingore the last newline.
                //
                using (MemoryStream bOut = new MemoryStream())
                {
                    int ch;

                    while ((ch = aIn.ReadByte()) >= 0 && aIn.IsClearText())
                    {
                        bOut.WriteByte((byte)ch);
                    }

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

                    using (MemoryStream lineOut = new MemoryStream())
                    {
                        using (Stream sigIn = new MemoryStream(bOut.ToArray(), false))
                        {
                            int lookAhead = ReadInputLine(lineOut, sigIn);

                            ProcessLine(sig, lineOut.ToArray());

                            if (lookAhead != -1)
                            {
                                do
                                {
                                    lookAhead = ReadInputLine(lineOut, lookAhead, sigIn);

                                    sig.Update((byte)'\r');
                                    sig.Update((byte)'\n');

                                    ProcessLine(sig, lineOut.ToArray());
                                }
                                while (lookAhead != -1);
                            }

                            return sig.Verify();
                        }
                    }
                }
            }
        }

        internal static string GetModulusData(string message)
        {
            using (ArmoredInputStream aIn = new ArmoredInputStream(
                new MemoryStream(Encoding.ASCII.GetBytes(message))))
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

                //
                // read the input, making sure we ingore the last newline.
                //
                int ch;

                List<byte> data = new List<byte>();

                while ((ch = aIn.ReadByte()) >= 0 && aIn.IsClearText())
                {
                    data.Add((byte)ch);
                }

                string bitString = Encoding.UTF8.GetString(data.ToArray(), 0, data.Count);
                return bitString.Trim();
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
