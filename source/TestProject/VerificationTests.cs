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

using Tuvi.Auth.Proton.Test.TestData;
using Tuvi.Proton.Primitive.Exceptions;
using Tuvi.Proton.Primitive.Modules;

namespace Tuvi.Auth.Proton.Test
{
    public class VerificationTests
    {
        [TestCase(VerificationData.RightSignedModulus, ExpectedResult = VerificationData.RightModulus)]
        [TestCase(VerificationData.RightSignedModulus2, ExpectedResult = VerificationData.RightModulus2)]
        [TestCase(VerificationData.RightSignedModulus3, ExpectedResult = VerificationData.RightModulus3)]
        public string ReadSignedMessageTest(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            return module.ReadSignedMessage(modulus);
        }

        [TestCase(VerificationData.WrongSignedModulus)]
        public void ReadSignedMessageArgumentExceptionTest(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            Assert.Throws<ArgumentException>(() => module.ReadSignedMessage(modulus));
        }

        [TestCase(VerificationData.BadHeaderModulus1)]
        [TestCase(VerificationData.BadHeaderModulus2)]
        public void ReadSignedMessageWrongHeaderException(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            Assert.Throws<WrongHeaderException>(() => module.ReadSignedMessage(modulus));
        }

        [TestCase(VerificationData.BadFormattedModulus1)]
        [TestCase(VerificationData.BadFormattedModulus2)]
        public void ReadSignedMessageIOException(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            Assert.Throws<IOException>(() => module.ReadSignedMessage(modulus));
        }

        [TestCase(VerificationData.WrongCRCModulus1)]
        [TestCase(VerificationData.WrongCRCModulus2)]
        public void ReadSignedMessageWrongCrcIOExceptionTest(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            Assert.Throws<IOException>(() => module.ReadSignedMessage(modulus));
        }

        [Test]
        public void DividedModulusCustomTest()
        {
            //Used private key
            /*
-----BEGIN PGP PRIVATE KEY BLOCK-----
Version: OpenPGP.js v.1.20130420
Comment: http://openpgpjs.org

xcA4BGQPGn4BAgDH3Ab7jD0uzAqKLF16y7f4Q8H5esFfDKDlHRpa61qGbWHA
4sgmP3tGDpi+5nlWAOppWes9I4xYNJm07M7SRtqJABEBAAEAAgCwp8NkRSkv
XBtU1rIqADe0se+a3O5fXYI6AedxxeZWOnixYAzYhyuQ8Ytz2Ps8DwTs0QTf
1nCHnbENPnNmrmkhAQDqopa75pDMyj6lSS2aFCSyZ4MmwPviqyYWXd1vXKE4
0wEA2g7PAf5UeY7Pb8bgObTbiu5nN6pQGz4NYPZ5d9TQBbMBAMb2lYV6lTSJ
noXk1MuFcAj9Vw+JuCovJ1rqDuAthGfQUdLNJFRlc3QgTWNUZXN0aW5ndG9u
IDx0ZXN0QGV4YW1wbGUuY29tPsJcBBABCAAQBQJkDxp/CRAwqf6B4dmClwAA
aLAB/i7q0zEwkHkPX7TW0q6XkSLvkd7jSUcInVLPoNa9ReE79Msj6XTiXBQm
fo+kNvSBUa5jXde/FIdNXbAmLgr08Qs=
=AF5d
-----END PGP PRIVATE KEY BLOCK-----
            */

            string pubKey = @"-----BEGIN PGP PUBLIC KEY BLOCK-----
Version: OpenPGP.js v.1.20130420
Comment: http://openpgpjs.org

xk0EZA8afgECAMfcBvuMPS7MCoosXXrLt/hDwfl6wV8MoOUdGlrrWoZtYcDi
yCY/e0YOmL7meVYA6mlZ6z0jjFg0mbTsztJG2okAEQEAAc0kVGVzdCBNY1Rl
c3Rpbmd0b24gPHRlc3RAZXhhbXBsZS5jb20+wlwEEAEIABAFAmQPGn8JEDCp
/oHh2YKXAABosAH+LurTMTCQeQ9ftNbSrpeRIu+R3uNJRwidUs+g1r1F4Tv0
yyPpdOJcFCZ+j6Q29IFRrmNd178Uh01dsCYuCvTxCw==
=msCp
-----END PGP PUBLIC KEY BLOCK-----";

            string modulus = @"-----BEGIN PGP SIGNED MESSAGE-----
Hash: SHA256

S/hBgmVXHlpzUxgzOlt4veE3v3BnpaVyRFUUDMmRgcF2yZ
U5rQcQYHDBGrnQAlGdcsGmZVcZC51JgJtEB6v5bBpxnnsj
g8XibZm0GYXODhm7qki5wM5AEKoTKbZKaKuRD297pPTsVd
qUdXFNdkDxk3Q3nv3N6ZEJccCS1IabllN+/adVTjUfCMA9
pyJavOOj90fhcCQ2npInsxegvlGvREr1JpobdrtbXAOzLH
+9ELxpW91ZFWbN0HHaE8+JV8TsZnhY+W0pqL+x18iVBwOC
KjqiNVlXsJsd4PV0fyX3Fb/uRTnUuEYe/98xo+qqG/CrhI
W7QgiuwemEN7PdHHARnQ==
-----BEGIN PGP SIGNATURE-----
Version: OpenPGP.js v1.0.1
Comment: http://openpgpjs.org

wlwEAQEIABAFAmQPIUcJEDCp/oHh2YKXAABqSwH/ZK/ugEmZsx5Qz1DZoRBV
loJxS/ZSaHHDiGdgQ2jrltyNe/6EEiGBmuep2en7nS8psgHGrCrBmIRzX9T7
P4yG4Q==
=x4hN
-----END PGP SIGNATURE-----";

            IPGPModule module = new StandardPGPModule();
            module.ImportKeys(pubKey);
            var mod = module.ReadSignedMessage(modulus);
            Assert.That(mod, Is.EqualTo(VerificationData.RightModulus));
        }
    }
}
