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

//using Tuvi.Proton.Primitive.Data;
using Tuvi.Proton.Primitive.Modules;

namespace Tuvi.Auth.Proton.Test.Data
{
    public class FakePGPModule : IPGPModule
    {
        public void ImportKeys(string keyData)
        {
            // fake
        }

        public string ReadSignedMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message can not be null or empty", nameof(message));
            }

            return GetModulusData(message);
        }

        private static string GetModulusData(string rawModulus)
        {
            var parts = rawModulus.Split('\n');

            if (parts.Length > 4)
            {
                return parts[3];
            }

            return string.Empty;
        }
    }
}
