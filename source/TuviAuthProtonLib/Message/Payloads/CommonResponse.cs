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

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Tuvi.Auth.Proton.Message.Payloads
{
    //  {
    //      "Code":9001,
    //      "Error":"For security reasons, please complete CAPTCHA. If you can't pass it, please try updating your app or contact us here: https://proton.me/support/abuse",
    //      "Details":
    //      {
    //          "HumanVerificationToken":"2ttdwwR9ckDp9XErbqp7bqLu",
    //          "HumanVerificationMethods":["captcha"],
    //          "Direct":1,
    //          "Description":"",
    //          "Title":"Human Verification"}
    //      }

    public class CommonResponse
    {
        // https://github.com/ProtonMail/WebClients/blob/main/packages/shared/lib/constants.ts
        // https://github.com/ProtonMail/WebClients/blob/main/packages/shared/lib/errors.js
        // https://github.com/ProtonMail/proton-mail-android/blob/release/app/src/main/java/ch/protonmail/android/api/segments/BaseApi.kt
        // https://github.com/ProtonMail/proton-python-client/blob/master/README.md
        public enum ResponseCode : int
        {
            Aborted = -1,
            Timeout = 0,

            UnprocessableEntity = 422,
            Unauthorized = 401,
            Unlock = 403,
            TooManyRequests = 429,
            BadGateway = 502,
            ServiceUnavailable = 503,
            GatwayTimeout = 504,

            SingleSuccess = 1000,
            GlobalSuccess = 1001,

            InvalidInput = 2001,

            InvalidAppVersion = 2064,
            MissingAppVersion = 5001,

            //ForceUpgrade = 5003,
            BodyRequestFailed = 6001,
            WrongPassword = 8002,
            //TooManyChildren = 8003,
            HumanVerificationRequired = 9001,
            AccountDeleted = 10002,
            AuthAccountDisabled = 10003,
            RefreshTokenInvalid = 10013,
            UserUpdateEmailSelf = 12007,
            TokenInvalid = 12087,
            KeyGetInputInvalid = 33101,
            KeyGetAddressMissing = 33102,
            KeyGetDomainMissingMX = 33103,
            IncomingDefaultUpdateNotExist = 35023,
            UserExistsUsernameAlreadyUsed = 12106,
            NoResetMethods = 2029,
            PaymentsSubscriptionAmountMismatch = 22101,
        }

        [JsonInclude]
        public int Code { get; internal set; }

        [JsonInclude]
        public string Error { get; internal set; }

        [JsonInclude]
        public JsonObject Details { get; internal set; }

        [JsonIgnore]
        public bool Success => ResponseCode.SingleSuccess.SameAs(Code) ||
                               ResponseCode.GlobalSuccess.SameAs(Code);
    }

    public static class ResponseCodeExtension
    {
        public static bool SameAs(this CommonResponse.ResponseCode code, int value)
        {
            return value == (int)code;
        }
    }
}
