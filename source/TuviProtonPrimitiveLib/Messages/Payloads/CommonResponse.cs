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

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Tuvi.Proton.Primitive.Messages.Payloads
{
    public class CommonResponse
    {
        // https://github.com/ProtonMail/WebClients/blob/main/packages/shared/lib/constants.ts
        // https://github.com/ProtonMail/WebClients/blob/main/packages/shared/lib/errors.js
        // https://github.com/ProtonMail/proton-mail-android/blob/release/app/src/main/java/ch/protonmail/android/api/segments/BaseApi.kt
        // https://github.com/ProtonMail/proton-python-client/blob/master/README.md
        // https://github.com/ProtonMail/go-proton-api/blob/master/response.go
        public enum ResponseCode : int
        {
            Aborted = -1,
            Timeout = 0,

            UnprocessableEntity = 422,
            Unauthorized = 401,
            RequestTimeout = 408,
            Unlock = 403,
            TooManyRequests = 429,
            BadGateway = 502,
            ServiceUnavailable = 503,
            GatwayTimeout = 504,

            SingleSuccess = 1000,
            MultiSuccess = 1001,

            InvalidValue = 2001,
            NotAllowed = 2011,
            MessageReadingRestricted = 2028,
            NoResetMethods = 2029,
            InvalidId = 2061,
            ErrorGroupAlreadyExist = 2500,

            InvalidAppVersion = 2064,
            MissingAppVersion = 5001,

            BadAppVersion = 5003,
            BodyRequestFailed = 6001,
            UsernameInvalid = 6003, // Deprecated, but still used.
            WrongPassword = 8002,
            TooManyChildren = 8003,
            HumanVerificationRequired = 9001,

            AuthAccountFailedGeneric = 10001,
            AuthAccountDeleted = 10002,
            AuthAccountDisabled = 10003,
            PaidPlanRequired = 10004,

            RefreshTokenInvalid = 10013,

            EmailFailedValidation = 12006,
            NewPasswordMessedUp = 12020,
            NewPasswordIncorrect = 12022,
            InvalidEmail = 12065,
            IncorrectPassword = 12066,
            TokenInvalid = 12087,

            ErrorContactExistThisEmail = 13002,
            ErrorInvalidEmail = 13006,
            ErrorEmailExist = 13007,
            ErrorEmailValidationFailed = 13014,
            ErrorEmailDuplicateFailed = 13061,

            DraftDoesNotExist = 15033,
            MessageAlreadySent = 15034,
            MessageDoesNotExist = 15052,

            RecipientNotFound = 33102,
        }

        [JsonInclude]
        public int Code { get; internal set; }

        [JsonInclude]
        public string Error { get; internal set; }

        [JsonInclude]
        public JsonObject Details { get; internal set; }

        [JsonIgnore]
        public bool Success => ResponseCode.SingleSuccess.SameAs(Code) ||
                               ResponseCode.MultiSuccess.SameAs(Code);

        public T ReadDetails<T>(JsonSerializerOptions options = null)
        {
            return JsonSerializer.Deserialize<T>(Details, options);
        }
    }

    public static class ResponseCodeExtension
    {
        public static bool SameAs(this CommonResponse.ResponseCode code, int value)
        {
            return value == (int)code;
        }
    }
}
