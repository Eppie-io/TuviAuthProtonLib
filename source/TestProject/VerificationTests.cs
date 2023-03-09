using Tuvi.Auth.Proton.Test.TestData;
using Tuvi.Auth.Services;
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
    }
}
