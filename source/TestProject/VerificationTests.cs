using Tuvi.Auth.Proton.Test.TestData;
using Tuvi.Auth.Services;
using Tuvi.Proton.Primitive.Exceptions;
using Tuvi.Proton.Primitive.Modules;

namespace Tuvi.Auth.Proton.Test
{
    public class VerificationTests
    {
        [TestCase(VerificationData.RightSignedModulus, ExpectedResult = true)]
        [TestCase(VerificationData.RightSignedModulus2, ExpectedResult = true)]
        [TestCase(VerificationData.RightSignedModulus3, ExpectedResult = true)]
        [TestCase(VerificationData.WrongSignedModulus, ExpectedResult = false)]
        public bool VerifyModulusTest(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            var verifiedModulus = module.VerifyModulus(modulus);
            return verifiedModulus.IsValid;
        }

        [TestCase(VerificationData.RightSignedModulus, ExpectedResult = VerificationData.RightSignedValue)]
        [TestCase(VerificationData.WrongSignedModulus, ExpectedResult = VerificationData.WrongSignedValue)]
        //[TestCase(VerificationData.BadFormattedModulus2, ExpectedResult = VerificationData.BadFormatValue)]
        public string GetModulusTest(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            var verifiedModulus = module.VerifyModulus(modulus);
            return verifiedModulus.Data;
        }

        [TestCase(VerificationData.BadHeaderModulus1)]
        [TestCase(VerificationData.BadHeaderModulus2)]
        public void VerifyModulusWrongHeaderException(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            Assert.Throws<WrongHeaderException>(() => module.VerifyModulus(modulus));
        }

        [TestCase(VerificationData.BadFormattedModulus1)]
        [TestCase(VerificationData.BadFormattedModulus2)]
        public void VerifyModulusIOException(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            Assert.Throws<IOException>(() => module.VerifyModulus(modulus));
        }

        [TestCase(VerificationData.WrongCRCModulus1)]
        [TestCase(VerificationData.WrongCRCModulus2)]
        public void VerifyModulusWrongCrcIOExceptionTest(string modulus)
        {
            IPGPModule module = new StandardPGPModule();
            Assert.Throws<IOException>(() => module.VerifyModulus(modulus));
        }

    }
}
