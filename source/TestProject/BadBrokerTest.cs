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
using Tuvi.Auth.Services;

namespace Tuvi.Auth.Proton.Test
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is instantiated via NUnit Framework")]
    internal class BadBrokerTest
    {
        private static readonly HttpClient _httpClient = new();
        public static Broker CreateBroker(string? appVersion = null, string? userAgent = null)
        {
            var protonSRPClient = new ProtonSRPClient(new FakePGPModule())
            {
                Fingerprint = Data.BrokerTestData.SRP_MODULUS_KEY_FINGERPRINT
            };

            return new Broker(_httpClient, protonSRPClient, Data.BrokerTestData.ProtonHostDefaultList[0])
            {
                AppVersion = appVersion,
                UserAgent = userAgent,
            };
        }

        [TearDown]
        public async Task PostTest()
        {
            await Task.Delay(5000).ConfigureAwait(false);
        }

        [Test]
        public void MissingAppVersionTest()
        {
            var broker = CreateBroker();
            AuthResponse? authResponse = null;

            var exception = Assert.ThrowsAsync<ProtonRequestException>(
                async () =>
                {
                    authResponse = await broker.AuthenticateAsync(
                        username: "any",
                        password: "any",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.That(authResponse, Is.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That((CommonResponse.ResponseCode)exception.ErrorInfo.Code, Is.EqualTo(CommonResponse.ResponseCode.MissingAppVersion));
            });
        }

        [Test]
        public void InvalidAppVersionTest()
        {
            var broker = CreateBroker("WrongAppVersion");
            AuthResponse? authResponse = null;

            var exception = Assert.ThrowsAsync<ProtonRequestException>(
                async () =>
                {
                    authResponse = await broker.AuthenticateAsync(
                        username: "any",
                        password: "any",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.That(authResponse, Is.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That((CommonResponse.ResponseCode)exception.ErrorInfo.Code, Is.EqualTo(CommonResponse.ResponseCode.InvalidAppVersion));
            });
        }

        [Test]
        public void CorrectAppVersionTest()
        {
            var broker = CreateBroker(Data.BrokerTestData.AppVersionDefault);
            AuthResponse? authResponse = null;

            var exception = Assert.ThrowsAsync<ProtonRequestException>(
                async () =>
                {
                    authResponse = await broker.AuthenticateAsync(
                        username: "any",
                        password: "any",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.That(authResponse, Is.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
                Assert.That((CommonResponse.ResponseCode)exception.ErrorInfo.Code, Is.EqualTo(CommonResponse.ResponseCode.HumanVerificationRequired));
            });
        }
    }
}