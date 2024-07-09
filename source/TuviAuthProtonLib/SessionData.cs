////////////////////////////////////////////////////////////////////////////////
//
//   Copyright 2024 Eppie(https://eppie.io)
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

namespace Tuvi.Auth.Proton
{
    public struct SessionData : IEquatable<SessionData>
    {
        public string Uid { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public long ExpirationTime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SessionData data && Equals(data);
        }

        public override int GetHashCode()
        {
            int hashCode = 1002824224;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Uid);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AccessToken);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TokenType);
            hashCode = hashCode * -1521134295 + ExpirationTime.GetHashCode();
            return hashCode;
        }

        public bool Equals(SessionData other)
        {
            return Uid == other.Uid &&
                   AccessToken == other.AccessToken &&
                   TokenType == other.TokenType &&
                   ExpirationTime == other.ExpirationTime;
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
