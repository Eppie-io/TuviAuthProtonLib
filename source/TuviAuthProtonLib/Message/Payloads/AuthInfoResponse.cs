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

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tuvi.Auth.Proton.Message.Payloads
{
    //  {
    //      "Code": 1000,
    //      "Modulus": "-----BEGIN PGP SIGNED MESSAGE-----\nHash: SHA256\n\nSample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample==\n-----BEGIN PGP SIGNATURE-----\nVersion: ProtonMail\nComment: https://protonmail.com\n\nSample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sam\nSample+S\nSampl\n-----END PGP SIGNATURE-----\n",
    //      "ServerEphemeral": "Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample+Sample==",
    //      "Version": 4,
    //      "Salt": "Sample+Sample+==",
    //      "SRPSession": "fffffff0123456789abcdeffffffffff"
    //  }

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
        Justification = "Class is instantiated via JsonSerializer")]
    internal class AuthInfoResponse : CommonResponse
    {
        [JsonInclude]
        public int Version { get; internal set; }

        [JsonInclude]
        public string Modulus { get; internal set; }

        [JsonInclude]
        public string ServerEphemeral { get; internal set; }

        [JsonInclude]
        public string Salt { get; internal set; }

        [JsonInclude]
        public string SRPSession { get; internal set; }
    }
}
