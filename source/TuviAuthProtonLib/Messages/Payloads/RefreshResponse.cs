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

using System.Text.Json.Serialization;
using Tuvi.Proton.Primitive.Messages.Payloads;

namespace Tuvi.Auth.Proton.Messages.Payloads
{
    public class RefreshResponse : CommonResponse
    {
        [JsonInclude]
        public string AccessToken { get; internal set; }

        [JsonInclude]
        public int ExpiresIn { get; internal set; }

        [JsonInclude]
        public string TokenType { get; internal set; }

        [JsonInclude]
        public string Scope { get; internal set; }

        [JsonInclude]
        public string Uid { get; internal set; } // headers["x-pm-uid"]

        [JsonInclude]
        [JsonPropertyName("UID")]
        public string SessionUid { get; internal set; } // it has the same value as the UID property

        [JsonInclude]
        public string RefreshToken { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("LocalID")]
        public int LocalID { get; internal set; }
    }
}
