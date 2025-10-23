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

namespace Tuvi.Auth.Proton.Exceptions
{
    public class AuthProtonArgumentException : AuthProtonException
    {
        internal AuthProtonArgumentException(string message) : base(message)
        { }

        internal AuthProtonArgumentException(string message, Exception innerException)
            : base(message, innerException)
        { }

        internal AuthProtonArgumentException(string message, string paramName)
            : base(GetMessage(message, paramName))
        { }

        internal AuthProtonArgumentException(string message, string paramName, Exception innerException)
            : base(GetMessage(message, paramName), innerException)
        { }

        internal AuthProtonArgumentException()
        { }

        private static string GetMessage(string message, string paramName)
        {
            return $"{message}{Environment.NewLine}Parameter name: {paramName}";
        }
    }
}
