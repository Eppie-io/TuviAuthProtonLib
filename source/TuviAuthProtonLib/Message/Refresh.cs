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
using System.Net.Http;

namespace Tuvi.Auth.Proton.Message
{
    internal class Refresh : PayloadMessage<Payloads.RefreshResponse, Refresh.Payload>
    {
        public override Uri Endpoint => new Uri("/auth/refresh", UriKind.Relative);
        public override HttpMethod Method => HttpMethod.Post;
        public Refresh(Payload payload)
            : base(payload)
        { }

        public struct Payload
        {
            public static string ResponseType => "token";
            public static string GrantType => "refresh_token";
            public string RefreshToken { get; set; }
            public string RedirectURI { get; set; }
        }
    }
}
