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
using System.Text.Json.Serialization;
using Tuvi.Proton.Primitive.Messages.Payloads;

namespace Tuvi.Proton.Primitive.Messages.Errors
{
    public class HumanVerificationDetails
    {
        [JsonInclude]
        public string HumanVerificationToken { get; private set; }

        [JsonInclude]
        public IList<string> HumanVerificationMethods { get; private set; }

        [JsonInclude]
        public int Direct { get; private set; }

        [JsonInclude]
        public string Description { get; private set; }

        [JsonInclude]
        public string Title { get; private set; }

        // https://github.com/ProtonMail/go-proton-api/blob/master/manager_user.go; line:8
        [JsonIgnore]
        public Uri CaptchaUri => new Uri($"/core/v4/captcha?Token={HumanVerificationToken}", UriKind.Relative);
    }

    public static partial class CommonResponseExtension
    {
        public static bool IsHumanVerificationRequired(this CommonResponse response)
        {
            var commonResponse = response ?? throw new ArgumentNullException(nameof(response));
            return CommonResponse.ResponseCode.HumanVerificationRequired.SameAs(commonResponse.Code);
        }
    }
}
