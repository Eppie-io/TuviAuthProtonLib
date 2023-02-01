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

namespace Tuvi.Auth.Proton.Test.Details
{
    //  {
    //      "HumanVerificationToken":"2ttdwwR9ckDp9XErbqp7bqLu",
    //      "HumanVerificationMethods":["captcha"],
    //      "Direct":1,
    //      "Description":"",
    //      "Title":"Human Verification"
    //  }

    internal readonly struct HumanVerification
    {
        public string HumanVerificationToken { get; init; }
        public IEnumerable<string> HumanVerificationMethods { get; init; }
        public int Direct { get; init; }
        public string Description { get; init; }
        public string Title { get; init; }
    }
}
