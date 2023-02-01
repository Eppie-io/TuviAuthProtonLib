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
using System.Net;
using System.Text.Json.Nodes;
using Tuvi.Auth.Proton.Message.Payloads;

namespace Tuvi.Auth.Exceptions
{
    public class ProtonRequestException : AuthProtonException
    {
        public HttpStatusCode HttpStatusCode { get; internal set; }
        public ErrorInfo ErrorInfo { get; internal set; }

        internal ProtonRequestException(string message) : base(message)
        { }

        internal ProtonRequestException(string message, Exception innerException)
            : base(message, innerException)
        { }

        internal ProtonRequestException()
        { }
    }

    public class ErrorInfo
    {
        public int Code { get; internal set; }
        public string Error { get; internal set; }
        public JsonObject Details { get; internal set; }

        internal ErrorInfo(CommonResponse response)
        {
            Code = response.Code;
            Error = response.Error;
            Details = response.Details;
        }
    }
}
