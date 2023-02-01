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
using Tuvi.Auth.Exceptions;
using Tuvi.Auth.Proton.Message;
using Tuvi.Auth.Proton.Message.Payloads;
using Tuvi.Auth.Services;
using Tuvi.RestClient;

namespace Tuvi.Auth.Proton
{
    public class Broker : Tuvi.RestClient.Client
    {
        public string ClientSecret { get; set; }
        public string UserAgent { get; set; }
        public string AppVersion { get; set; }

        private readonly ISRPClient _srpClient;

        public Broker(HttpClient httpClient, ISRPClient srpClient, Uri host)
            : base(httpClient, host)
        {
            _srpClient = srpClient;
        }

        public async Task<Message.Payloads.AuthResponse> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                AuthProtonException.ThrowSystemException(
                    message: "Authentication could not be done",
                    exception: new ArgumentException("The username is required", nameof(username))
                );
            }

            if (string.IsNullOrEmpty(password))
            {
                AuthProtonException.ThrowSystemException(
                    message: "Authentication could not be done",
                    exception: new ArgumentException("The password cannot not be null or empty", nameof(password))
                );
            }

            var msgAuthInfo = new Message.AuthInfo(
            payload: new Message.AuthInfo.Payload
            {
                Username = username,
                ClientSecret = this.ClientSecret
            });

            var response = await SendMessageAsync(msgAuthInfo, default, cancellationToken).ConfigureAwait(false);
            var proof = _srpClient.CalculateProof(
                version: response.Version,
                username: username,
                password: password,
                salt: response.Salt,
                modulus: response.Modulus,
                serverEphemeral: response.ServerEphemeral);

            var msgAuth = new Message.Auth(
                payload: new Message.Auth.Payload
                {
                    Username = username,
                    ClientEphemeral = proof.ClientEphemeral,
                    ClientProof = proof.ClientProof,
                    SRPSession = response.SRPSession
                });

            var result = await SendMessageAsync(msgAuth, default, cancellationToken).ConfigureAwait(false);
            if (!result.Success)
            {
                throw new AuthProtonException("Invalid server proof");
            }

            if (_srpClient.VerifySession(result.ServerProof) is false)
            {
                throw new AuthProtonException("Invalid server proof");
            }

            return result;
        }

        public Task<Message.Payloads.TwoFactorCodeResponse> ProvideTwoFactorCodeAsync(SessionData sessionData, string code, CancellationToken cancellationToken)
        {
            var message = new Message.TwoFactorCode(new TwoFactorCode.Payload
            {
                TwoFactorCode = code
            });
            return SendMessageAsync(message, sessionData, cancellationToken);
        }

        public Task<Message.Payloads.CommonResponse> LogoutAsync(SessionData sessionData, CancellationToken cancellationToken)
        {
            var message = new Message.Logout();
            return SendMessageAsync(message, sessionData, cancellationToken);
        }

        public Task<Message.Payloads.RefreshResponse> RefreshAsync(SessionData sessionData, string RefreshToken, CancellationToken cancellationToken)
        {
            var message = new Message.Refresh(new Refresh.Payload()
            {
                RedirectURI = "",
                RefreshToken = RefreshToken
            });
            return SendMessageAsync(message, sessionData, cancellationToken);
        }

        private async Task<TResponsePayload> SendMessageAsync<TResponsePayload, TRequest>(HeaderMessage<JsonResponse<TResponsePayload>, TRequest> message, SessionData sessionData, CancellationToken cancellationToken)
            where TRequest : Request
            where TResponsePayload : CommonResponse
        {
            if (message is null)
            {
                AuthProtonException.ThrowSystemException(
                    message: "Message could not be sent",
                    exception: new ArgumentNullException(nameof(message))
                );
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                AddHeaders(message, sessionData);

                await SendAsync(message, cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                return message.Response.Content;
            }
            catch (HttpRequestException exception)
            {
                throw new ProtonRequestException("Bad Proton Request", exception)
                {
                    HttpStatusCode = message.HttpStatus,
                    ErrorInfo = new ErrorInfo(message.Response.Content)
                };
            }
        }

        private void AddHeaders<TResponse, TRequest>(HeaderMessage<TResponse, TRequest> message, SessionData sessionData)
            where TResponse : Response
            where TRequest : Request
        {
            message.UserAgent = UserAgent;
            message.AppVersion = AppVersion;
            message.TokenType = sessionData.TokenType;
            message.AccessToken = sessionData.AccessToken;
            message.Uid = sessionData.Uid;
        }
    }

    public struct SessionData : IEquatable<SessionData>
    {
        public string Uid { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }

        public bool Equals(SessionData other)
        {
            return string.Equals(Uid, other.Uid, StringComparison.Ordinal) &&
                   string.Equals(TokenType, other.TokenType, StringComparison.Ordinal) &&
                   string.Equals(AccessToken, other.AccessToken, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (obj is SessionData sessionData)
                return Equals(sessionData);

            return false;
        }

        public override int GetHashCode()
        {
            return $"#{Uid}#{TokenType}#{AccessToken}".GetHashCode();
        }

        public static bool operator ==(SessionData left, SessionData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SessionData left, SessionData right)
        {
            return !left.Equals(right);
        }
    }
}
