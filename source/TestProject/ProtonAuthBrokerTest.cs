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

using System.Diagnostics.CodeAnalysis;
using System.Net;
using Tuvi.Auth.Exceptions;
using Tuvi.Auth.Proton.Message.Payloads;
using Tuvi.Auth.Proton.Test.Data;
using Tuvi.Auth.Proton.Test.Details;
using Tuvi.Auth.Services;

namespace Tuvi.Auth.Proton.Test
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is instantiated via NUnit Framework")]
    [TestFixtureSource(typeof(BrokerTestData), nameof(BrokerTestData.BrokerConfigParams))]
    internal class BrokerTests
    {
        private BrokerTestData.TestConfiguration Config { get; init; }
        private Broker ProtonAuthBroker { get; set; }

        public BrokerTests(BrokerTestData.TestConfiguration config)
        {
            Config = config;
        }

        [SetUp]
        public void Setup()
        {
            var protonSRPClient = new ProtonSRPClient(new FakePGPModule())
            {
                Fingerprint = Config.Fingerprint
            };

            ProtonAuthBroker = new Broker(Config.HttpClient, protonSRPClient, Config.Host)
            {
                AppVersion = Config.AppVersion,
                UserAgent = Config.UserAgent,
            };
        }

        [TearDown]
        public async Task PostTest()
        {
            await Task.Delay(5000).ConfigureAwait(false);
        }

        [Test]
        //[Ignore("Too Many Requests")]
        public void AuthenticateTest()
        {
            AuthResponse? response = null;

            Assert.DoesNotThrowAsync(
                code: async () =>
                {
                    response = await ProtonAuthBroker.AuthenticateAsync(
                        username: BrokerTestData.User,
                        password: BrokerTestData.Password,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That((CommonResponse.ResponseCode)response.Code, Is.EqualTo(CommonResponse.ResponseCode.SingleSuccess));
                Assert.That(response.TwoFactor, Is.Not.Zero);
                Assert.That(response.TokenType, Is.EqualTo("Bearer"));
            });
        }

        [Test]
        public void NullTest()
        {
            Assert.ThrowsAsync<AuthProtonException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: null,
                        password: null,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: "any",
                        password: null,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: null,
                        password: "any",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: string.Empty,
                        password: string.Empty,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: "any",
                        password: string.Empty,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: string.Empty,
                        password: "any",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: " ",
                        password: "any",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });
        }


        [Test]
        public void IncorrectUserTest()
        {
            AuthResponse? response = null;

            var exception = Assert.ThrowsAsync<ProtonRequestException>(
                async () =>
                {
                    response = await ProtonAuthBroker.AuthenticateAsync(
                        username: "unknown@proton.me",
                        password: "wrong-password",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.That(response, Is.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
                Assert.That((CommonResponse.ResponseCode)exception.ErrorInfo.Code, Is.EqualTo(CommonResponse.ResponseCode.HumanVerificationRequired));

                var humanVerification = System.Text.Json.JsonSerializer.Deserialize<HumanVerification>(exception.ErrorInfo.Details);
                Assert.That(humanVerification.HumanVerificationToken, Is.Not.Null.And.Not.Empty);
            });
        }

        [Test]
        public void WrongPasswordTest()
        {
            AuthResponse? response = null;

            var exception = Assert.ThrowsAsync<ProtonRequestException>(
                async () =>
                {
                    response = await ProtonAuthBroker.AuthenticateAsync(
                        username: BrokerTestData.User,
                        password: "wrong-password",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.That(response, Is.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
                Assert.That((CommonResponse.ResponseCode)exception.ErrorInfo.Code, Is.EqualTo(CommonResponse.ResponseCode.WrongPassword));
            });
        }

        [Test]
        public void BadRequestTest()
        {
            RefreshResponse? response = null;

            var exception = Assert.ThrowsAsync<ProtonRequestException>(
                async () =>
                {
                    response = await ProtonAuthBroker.RefreshAsync(
                        userData: new UserData
                        {
                            Uid = "incorrect",
                        },
                        RefreshToken: "incorrect",
                        cancellationToken: CancellationToken.None
                        ).ConfigureAwait(false);
                });

            Assert.That(response, Is.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
                Assert.That((CommonResponse.ResponseCode)exception.ErrorInfo.Code, Is.EqualTo(CommonResponse.ResponseCode.RefreshTokenInvalid));
            });


            exception = Assert.ThrowsAsync<ProtonRequestException>(
                async () =>
                {
                    response = await ProtonAuthBroker.RefreshAsync(
                        userData: new UserData(),
                        RefreshToken: "incorrect",
                        cancellationToken: CancellationToken.None
                        ).ConfigureAwait(false);
                });

            Assert.That(response, Is.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That((CommonResponse.ResponseCode)exception.ErrorInfo.Code, Is.EqualTo(CommonResponse.ResponseCode.InvalidInput));
            });
        }

        [Test]
        public void UnauthorizedRequestTest()
        {
            TwoFactorCodeResponse? response = null;

            var exception = Assert.ThrowsAsync<ProtonRequestException>(
                async () =>
                {
                    response = await ProtonAuthBroker.ProvideTwoFactorCodeAsync(
                        userData: new UserData(),
                        code: "000000",
                        cancellationToken: CancellationToken.None
                        ).ConfigureAwait(false);
                });

            Assert.That(response, Is.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                Assert.That((CommonResponse.ResponseCode)exception.ErrorInfo.Code, Is.EqualTo(CommonResponse.ResponseCode.Unauthorized));
            });

            CommonResponse? logoutResponse = null;

            exception = Assert.ThrowsAsync<ProtonRequestException>(
                async () =>
                {
                    logoutResponse = await ProtonAuthBroker.LogoutAsync(
                        userData: new UserData(),
                        cancellationToken: CancellationToken.None
                        ).ConfigureAwait(false);
                });

            Assert.That(logoutResponse, Is.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                Assert.That((CommonResponse.ResponseCode)exception.ErrorInfo.Code, Is.EqualTo(CommonResponse.ResponseCode.Unauthorized));
            });
        }
    }
}