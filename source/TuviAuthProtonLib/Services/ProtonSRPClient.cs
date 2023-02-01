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
using Tuvi.Auth.Exceptions;
using TuviSRPLib.Utils;

namespace Tuvi.Auth.Services
{
    public class ProtonSRPClient : ISRPClient
    {
        private readonly IPGPModule _pgpModule;

        public string Fingerprint { get; set; }

        public ProtonSRPClient(IPGPModule pgpModule)
        {
            _pgpModule = pgpModule;
        }

        public Data.IProof CalculateProof(int version, string username, string password, string salt, string modulus, string serverEphemeral)
        {
            if (string.IsNullOrEmpty(modulus))
            {
                AuthProtonException.ThrowSystemException(
                    message: "Proof could not be calculated",
                    exception: new ArgumentException("Modulus cannot be null or empty", nameof(modulus))
                    );
            }

            var verified = _pgpModule.VerifyModulus(modulus);

            if (verified is null)
            {
                throw new AuthProtonException("Modulus cannot not be verified");
            }

            if (verified.IsValid is false)
            {
                throw new AuthProtonException("Invalid modulus");
            }

            if (string.IsNullOrEmpty(Fingerprint) || !Fingerprint.Equals(verified.Fingerprint, StringComparison.OrdinalIgnoreCase))
            {
                throw new AuthProtonException("Fingerprint is incorrect");
            }

            var srpClient = new TuviSRPLib.ProtonSRPClient();
            srpClient.SimpleInit(verified.Data);

            var ephemeral = srpClient.GenerateClientCredentials(salt, password);
            srpClient.CalculateSecret(serverEphemeral);

            var proof = srpClient.CalculateClientEvidenceMessage();

            return new Proof
            {
                ClientEphemeral = ephemeral.ToBase64(),
                ClientProof = proof.ToBase64()
            };
        }

        public bool VerifySession(string ServerProof)
        {
            //ToDo: Need implementation.
            return true;
        }

        internal struct Proof : Data.IProof
        {
            public string ClientEphemeral { get; set; }
            public string ClientProof { get; set; }
        }
    }
}
