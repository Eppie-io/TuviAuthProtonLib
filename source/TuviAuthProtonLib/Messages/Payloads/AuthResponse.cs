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
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Tuvi.Proton.Primitive.Messages.Payloads;

namespace Tuvi.Auth.Proton.Messages.Payloads
{
    public class AuthResponse : CommonResponse
    {
        [JsonInclude]
        public int ExpiresIn { get; internal set; }

        [JsonInclude]
        public int LocalID { get; internal set; }

        [JsonInclude]
        public string TokenType { get; internal set; } = "Bearer";

        [JsonInclude]
        public string AccessToken { get; internal set; } // headers["Authorization"]

        [JsonInclude]
        public string RefreshToken { get; internal set; }

        [JsonInclude]
        public IEnumerable<string> Scopes { get; internal set; }

        [JsonInclude]
        public string UserID { get; internal set; }

        [JsonInclude]
        public string EventID { get; internal set; }

        [JsonInclude]
        public int PasswordMode { get; internal set; }

        [JsonInclude]
        public string ServerProof { get; internal set; }

        [JsonInclude]
        public string Scope { get; internal set; }

        [JsonInclude]
        public string UID { get; internal set; } // headers["x-pm-uid"]

        [JsonInclude]
        public int TwoFactor { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("2FA")]
        public TwoFactorSettings TwoFactorSettings { get; internal set; }

        [JsonInclude]
        public int TemporaryPassword { get; internal set; }
    }

    public class TwoFactorSettings
    {
        [JsonInclude]
        public int Enabled { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("FIDO2")]
        public JsonObject Fido2 { get; internal set; }

        [JsonInclude]
        public int TOTP { get; internal set; }
    }

    [Flags]
    public enum TwoFAStatus
    {
        None = 0,
        TOTP = 1 << 0,
        FIDO2 = 1 << 1,
    }

    public enum PasswordMode
    {
        UnknownMode = 0,
        OnePasswordMode,
        TwoPasswordMode,
    }
}
