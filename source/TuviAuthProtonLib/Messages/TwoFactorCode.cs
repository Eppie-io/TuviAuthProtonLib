﻿////////////////////////////////////////////////////////////////////////////////
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
using System.Net.Http;
using Tuvi.Proton.Primitive.Messages;

namespace Tuvi.Auth.Proton.Messages
{
    internal class TwoFactorCode : PayloadMessage<Payloads.TwoFactorCodeResponse, TwoFactorCode.Payload>
    {
        public override Uri Endpoint => new Uri("/auth/v4/2fa", UriKind.Relative);
        public override HttpMethod Method => HttpMethod.Post;
        public TwoFactorCode(Payload payload)
            : base(payload)
        { }

        public struct Payload
        {
            public string TwoFactorCode { get; set; }
        }
    }
}
