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

namespace Tuvi.Auth.Proton
{
    public struct SessionData : IEquatable<SessionData>
    {
        public string Uid { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }

        public bool Equals(SessionData other)
        {
            return string.Equals(Uid, other.Uid, StringComparison.Ordinal) &&
                   string.Equals(TokenType, other.TokenType, StringComparison.Ordinal) &&
                   string.Equals(AccessToken, other.AccessToken, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (obj is SessionData sessionData)
                return Equals(sessionData);

            return false;
        }

        public override int GetHashCode()
        {
            return $"#{Uid}#{TokenType}#{AccessToken}".GetHashCode();
        }

        public static bool operator ==(SessionData left, SessionData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SessionData left, SessionData right)
        {
            return !left.Equals(right);
        }
    }
}
