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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tuvi.Auth.Proton.Exceptions;
using Tuvi.Auth.Services;
using Tuvi.Proton.Primitive.Messages;
using Tuvi.Proton.Primitive.Messages.Payloads;
using Tuvi.RestClient;

namespace Tuvi.Auth.Proton
{
    public class Broker : Tuvi.RestClient.Client
    {
        public string ClientSecret { get; set; }
        public Uri RedirectUri { get; set; }
        public string UserAgent { get; set; }
        public string AppVersion { get; set; }
        public string HumanVerificationToken { get; set; }
        public string HumanVerificationTokenType { get; set; }


        private readonly ISRPClientFactory _srpClientFactory;

        public Broker(HttpClient httpClient, ISRPClientFactory srpClientFactory, Uri host)
            : base(httpClient, host)
        {
            if (srpClientFactory is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            _srpClientFactory = srpClientFactory;
        }

        public Broker(HttpClient httpClient, Uri host)
            : base(httpClient, host)
        {
            _srpClientFactory = new StandardSRPClientFactory();
        }

        public async Task<Func<CancellationToken, Task<Messages.Payloads.AuthResponse>>> BuildAuthenticatorAsync(string username, string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new AuthProtonArgumentException(
                    message: "Authentication could not be done. The username is required.",
                    paramName: nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new AuthProtonArgumentException(
                    message: "Authentication could not be done. The password is required.",
                    paramName: nameof(password));
            }

            var msgAuthInfo = new Messages.AuthInfo(
            payload: new Messages.AuthInfo.Payload
            {
                Username = username,
                ClientSecret = this.ClientSecret
            });

            var response = await SendMessageAsync(msgAuthInfo, default, cancellationToken).ConfigureAwait(false);

            var srpClient = _srpClientFactory.CreateClient() ?? throw new AuthProtonException("SRP Client is not created.");
            var proof = srpClient.CalculateProof(
                version: response.Version,
                username: username,
                password: password,
                salt: response.Salt,
                modulus: response.Modulus,
                serverEphemeral: response.ServerEphemeral);

            var payload = new Messages.Auth.Payload
            {
                Username = username,
                ClientEphemeral = proof.ClientEphemeral,
                ClientProof = proof.ClientProof,
                SRPSession = response.SRPSession,
                ClientSecret = this.ClientSecret
            };

            return (ct) =>
            {
                return AuthenticateAsync(this, srpClient, payload, ct);
            };
        }

        public Task<Messages.Payloads.TwoFactorCodeResponse> ProvideTwoFactorCodeAsync(SessionData sessionData, string code, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new AuthProtonArgumentException(
                    message: "Two-factor authentication could not be done. The code is required.",
                    paramName: nameof(code));
            }

            var message = new Messages.TwoFactorCode(new Messages.TwoFactorCode.Payload
            {
                TwoFactorCode = code
            });
            return SendMessageAsync(message, sessionData, cancellationToken);
        }

        public Task<CommonResponse> LogoutAsync(SessionData sessionData, CancellationToken cancellationToken)
        {
            var message = new Messages.Logout();
            return SendMessageAsync(message, sessionData, cancellationToken);
        }

        public Task<Messages.Payloads.RefreshResponse> RefreshAsync(SessionData sessionData, string RefreshToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(RefreshToken))
            {
                throw new AuthProtonArgumentException(
                    message: "Refresh could not be done. The refresh token is required.",
                    paramName: nameof(RefreshToken));
            }

            var message = new Messages.Refresh(new Messages.Refresh.Payload()
            {
                ResponseType = "token",
                GrantType = "refresh_token",
                RedirectUri = RedirectUri,
                RefreshToken = RefreshToken,
            });
            return SendMessageAsync(message, sessionData, cancellationToken);
        }

        private async Task<TResponsePayload> SendMessageAsync<TResponsePayload, TRequest>(
            ProtonMessage<JsonResponse<TResponsePayload>, TRequest> message,
            SessionData sessionData,
            CancellationToken cancellationToken)

            where TRequest : Request
            where TResponsePayload : CommonResponse
        {
            if (message is null)
            {
                throw new AuthProtonArgumentException(
                    message: "Message could not be sent.",
                    paramName: nameof(message));
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                AddHeaders(message, sessionData);

                await SendAsync(message, cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                if (message.Response?.Content is null)
                {
                    throw new AuthProtonResponseException(
                        message: "Expected content not found.",
                        code: message.HttpStatus);
                }

                return message.Response.Content;
            }
            catch (HttpRequestException exception)
            {
                throw new AuthProtonRequestException(
                    message: "Bad Proton request.",
                    innerException: exception,
                    code: message.HttpStatus,
                    response: message.Response?.Content);
            }
        }

        private void AddHeaders<TResponse, TRequest>(ProtonMessage<TResponse, TRequest> message, SessionData sessionData)
            where TResponse : Response
            where TRequest : Request
        {
            message.UserAgent = UserAgent;
            message.AppVersion = AppVersion;
            message.TokenType = sessionData.TokenType;
            message.AccessToken = sessionData.AccessToken;
            message.Uid = sessionData.Uid;
            message.HumanVerificationToken = HumanVerificationToken;
            message.HumanVerificationTokenType = HumanVerificationTokenType;
        }

        private static async Task<Messages.Payloads.AuthResponse> AuthenticateAsync(Broker broker, ISRPClient srpClient, Messages.Auth.Payload payload, CancellationToken cancellationToken)
        {
            var msgAuth = new Messages.Auth(payload: payload);

            var result = await broker.SendMessageAsync(msgAuth, default, cancellationToken).ConfigureAwait(false);
            if (result.Success is false)
            {
                throw new AuthUnsuccessProtonException(result.Error, result);
            }

            if (srpClient.VerifySession(result.ServerProof) is false)
            {
                throw new AuthProtonException("Invalid server proof.");
            }

            return result;
        }
    }
}
