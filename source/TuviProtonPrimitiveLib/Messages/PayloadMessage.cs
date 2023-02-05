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
using System.Text.Json.Serialization;
using Tuvi.RestClient;

namespace Tuvi.Proton.Primitive.Messages
{
    public abstract class PayloadMessage<TResponsePayload, TRequestPayload>
        : ProtonMessage<JsonResponse<TResponsePayload>, JsonRequest<TRequestPayload>>
    {
        private readonly TRequestPayload _payload;

        protected PayloadMessage(TRequestPayload payload)
        {
            _payload = payload;
        }

        protected override JsonRequest<TRequestPayload> CreateRequest()
        {
            return new JsonRequest<TRequestPayload>
            {
                Payload = _payload,
                Options = new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                },
                Headers = BuildHeaders()
            };
        }

        protected override JsonResponse<TResponsePayload> CreateResponse()
        {
            return new JsonResponse<TResponsePayload>
            {
                Options = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                }
            };
        }
    }

    public abstract class PayloadMessage<TResponsePayload>
        : ProtonMessage<JsonResponse<TResponsePayload>, EmptyRequest>
    {
        protected override EmptyRequest CreateRequest()
        {
            return new EmptyRequest
            {
                Headers = BuildHeaders()
            };
        }

        protected override JsonResponse<TResponsePayload> CreateResponse()
        {
            return new JsonResponse<TResponsePayload>
            {
                Options = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                }
            };
        }
    }
}
