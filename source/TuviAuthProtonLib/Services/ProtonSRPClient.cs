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

using System;
using Tuvi.Auth.Proton;
using Tuvi.Auth.Proton.Exceptions;
using Tuvi.Proton.Primitive.Modules;
using TuviSRPLib.Utils;

namespace Tuvi.Auth.Services
{
    public class ProtonSRPClient : ISRPClient
    {
        private readonly IPGPModule _pgpModule;
        private readonly TuviSRPLib.ProtonSRPClient _srpClient = new TuviSRPLib.ProtonSRPClient();

        public string Fingerprint { get; } = Constants.SRP_MODULUS_KEY_FINGERPRINT;

        public ProtonSRPClient(IPGPModule pgpModule)
        {
            _pgpModule = pgpModule;
        }

        public Data.IProof CalculateProof(int version, string username, string password, string salt, string modulus, string serverEphemeral)
        {
            if (version < 3)
            {
                throw new AuthProtonArgumentException(
                    message: "Unsupported auth version.",
                    paramName: nameof(version));
            }

            if (string.IsNullOrEmpty(modulus))
            {
                throw new AuthProtonArgumentException(
                    message: "Proof could not be calculated. The modulus is required.",
                    paramName: nameof(modulus));
            }

            var verified = _pgpModule.VerifyModulus(modulus);

            if (verified is null)
            {
                throw new AuthProtonException("Modulus cannot not be verified.");
            }

            if (verified.IsValid is false)
            {
                throw new AuthProtonException("Invalid modulus.");
            }

            //if (string.IsNullOrEmpty(Fingerprint) || !Fingerprint.Equals(verified.Fingerprint, StringComparison.OrdinalIgnoreCase))
            //{
            //    throw new AuthProtonException("Fingerprint is incorrect.");
            //}

            try
            {
                _srpClient.SimpleInit(verified.Data);

                var ephemeral = _srpClient.GenerateClientCredentials(salt, password);
                _srpClient.CalculateSecret(serverEphemeral);

                var proof = _srpClient.CalculateClientEvidenceMessage();

                return new Proof
                {
                    ClientEphemeral = ephemeral.ToBase64(),
                    ClientProof = proof.ToBase64()
                };
            }
            catch (Exception ex)
            {
                throw new AuthProtonException("Proof cannot not be calculated. Some data is corrupted or missing.", innerException: ex);
            }
        }

        public bool VerifySession(string serverProof)
        {
            return _srpClient.VerifyServerEvidenceMessage(serverProof);
        }

        internal struct Proof : Data.IProof
        {
            public string ClientEphemeral { get; set; }
            public string ClientProof { get; set; }
        }
    }
}
