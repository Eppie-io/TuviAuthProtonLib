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
using Tuvi.Auth.Proton.Exceptions;
using Tuvi.Proton.Primitive.Modules;
using TuviSRPLib.Utils;

namespace Tuvi.Auth.Services
{
    public class ProtonSRPClient : ISRPClient
    {
        private readonly IPGPModule _pgpModule;
        private readonly TuviSRPLib.ProtonSRPClient _srpClient = new TuviSRPLib.ProtonSRPClient();

        public ProtonSRPClient()
        {
            _pgpModule = new StandardPGPModule();
        }

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

            string verifiedModulus;

            try
            {
                verifiedModulus = _pgpModule.ReadSignedMessage(modulus);
            }
            catch (Exception ex)
            {
                throw new AuthProtonException(ex.Message);
            }

            try
            {
                _srpClient.SimpleInit(verifiedModulus);

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
