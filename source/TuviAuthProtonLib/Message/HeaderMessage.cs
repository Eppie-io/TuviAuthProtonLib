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
using Tuvi.RestClient;

namespace Tuvi.Auth.Proton.Message
{
    internal abstract class HeaderMessage<TResponse, TRequest> : CommonMessage<TResponse, TRequest>
        where TResponse : Response
        where TRequest : Request
    {
        public string UserAgent { get; set; }
        public string AppVersion { get; set; }
        public string Uid { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }

        protected virtual HeaderCollection BuildHeaders()
        {
            var headers = new List<(string, string)>();

            if (!string.IsNullOrWhiteSpace(UserAgent))
            {
                headers.Add((Headers.ProtonHeader.UserAgentHeaderName, UserAgent));
            }

            if (!string.IsNullOrWhiteSpace(AppVersion))
            {
                headers.Add((Headers.ProtonHeader.AppVersionHeaderName, AppVersion));
            }

            if (!string.IsNullOrWhiteSpace(Uid))
            {
                headers.Add((Headers.ProtonHeader.UidHeaderName, Uid));
            }

            if (!string.IsNullOrWhiteSpace(AccessToken))
            {
                headers.Add((Headers.ProtonHeader.AuthorizationHeaderName, $"{TokenType} {AccessToken}"));
            }

            return new HeaderCollection(headers);
        }
    }
}
