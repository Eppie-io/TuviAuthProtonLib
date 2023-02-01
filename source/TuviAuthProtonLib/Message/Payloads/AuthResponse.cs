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

using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Tuvi.Auth.Proton.Message.Payloads
{
    //  {
    //      "Code":1000,
    //      "ExpiresIn":86400,
    //      "LocalID":0,
    //      "TokenType":"Bearer",
    //      "AccessToken":"yg5popf7oye6fctwcuzcmd5yqmnzn4tx",
    //      "RefreshToken":"ledyed3oqisi7rrkfdov7qujmpayo3zc",
    //      "Scopes":["self","parent","user","twofactor","vpn"],
    //      "UID":"o5pjje6pq6kz6erw24xtr3jbgqawru34",
    //      "UserID":"Hz0SBbeLVDOYLbEayv9McuKAfgOVncaiC5JrJKI--RqiO-ZilGU0f-Bzugxoff2QfOIC24ZxnqKdFMG_RdbJ1w==",
    //      "EventID":"Ll-58ES06esls1TwD_4_uuwGNeBfri133lR5R62ok-qXl75AT6PzGJ6rxtT_yPjA7JPECH1o7ki8UgqEq4P1hA==",
    //      "PasswordMode":1,
    //      "ServerProof":"QGDmtBTH61gdqGlfOuxTD3nqxKTey/1oEPYOHAeUd7uHPQDKpC+NlC8XMAbPzQjgA73gc2YN0xzIaZrWyqUnyZwqHov9WmFWMIlDZAP1NwuIS7n4PhQH8LEd631yiDYw0eyPImTbrpyxmJUYwfnzbJ6OGTeSkNJ3Hdj2MO7AnUPNOj/dxIAIGraVUtTVlws4E87NiuTbIrZ+1i1HTgs6ePqXwwA5z4LPPcL1LBjgwHQuEfN3rVuaAFOqvovGX1dAAXEiZ7Ttz3Mwka7d7FcCdxhMHx4S/VhasjR7G7PJoT79G3544fpxm/aQr20MYkgJxMhBcceeaaj0Muh1vaIhNQ==",
    //      "Scope":"self parent user twofactor vpn",
    //      "Uid":"o5pjje6pq6kz6erw24xtr3jbgqawru34",
    //      "TwoFactor":1,
    //      "2FA":
    //      {
    //          "Enabled":1,
    //          "FIDO2":{"AuthenticationOptions":null,"RegisteredKeys":[]},
    //          "TOTP":1
    //      },
    //      "TemporaryPassword":0}
    //  }

    public class AuthResponse : CommonResponse
    {
        [JsonInclude]
        public int ExpiresIn { get; internal set; }

        [JsonInclude]
        public int LocalID { get; internal set; }

        [JsonInclude]
        public string TokenType { get; internal set; }

        [JsonInclude]
        public string AccessToken { get; internal set; } // headers["Authorization"]

        [JsonInclude]
        public string RefreshToken { get; internal set; }

        [JsonInclude]
        public IEnumerable<string> Scopes { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("UID")]
        public string SessionUid { get; internal set; } // headers["x-pm-uid"]

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
        public string Uid { get; internal set; } // it has the same value as the UID property

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
}
