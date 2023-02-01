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

namespace Tuvi.Auth.Headers
{
    // https://github.com/ProtonMail/WebClients/blob/main/packages/shared/lib/fetch/headers.ts
    public static class ProtonHeader
    {
        public static string AppVersionHeaderName => "x-pm-appversion";
        public static string UidHeaderName => "x-pm-uid";

        public static string HumanVerificationTokenName => "x-pm-human-verification-token";
        public static string HumanVerificationTokenTypeName => "x-pm-human-verification-token-type";
        public static string LocaleName => "x-pm-locale";

        internal static string UserAgentHeaderName => "User-Agent";
        internal static string AuthorizationHeaderName => "Authorization";
    }
}
