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
using Tuvi.Auth.Proton.Exceptions;
using Tuvi.Auth.Proton.Test.Data;

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
            ProtonAuthBroker = new Broker(Config.HttpClient, new SRPClientFactory(), Config.Host)
            {
                AppVersion = Config.AppVersion,
                UserAgent = Config.UserAgent,
            };
        }

        [Test]
        public void AuthenticateAsync_WrongArgument_Throws()
        {
            Assert.ThrowsAsync<AuthProtonArgumentException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: null,
                        password: null,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonArgumentException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: "any",
                        password: null,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonArgumentException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: null,
                        password: "any",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonArgumentException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: string.Empty,
                        password: string.Empty,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonArgumentException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: "any",
                        password: string.Empty,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonArgumentException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: string.Empty,
                        password: "any",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });

            Assert.ThrowsAsync<AuthProtonArgumentException>(
                async () =>
                {
                    await ProtonAuthBroker.AuthenticateAsync(
                        username: " ",
                        password: "any",
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });
        }

        [Test]
        public void RefreshAsync_WrongArgument_Throws()
        {
            Assert.ThrowsAsync<AuthProtonArgumentException>(
                async () =>
                {
                    await ProtonAuthBroker.RefreshAsync(
                        sessionData: new SessionData(),
                        RefreshToken: null,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });
        }

        [Test]
        public void ProvideTwoFactorCodeAsync_WrongArgument_Throws()
        {
            Assert.ThrowsAsync<AuthProtonArgumentException>(
                async () =>
                {
                    await ProtonAuthBroker.ProvideTwoFactorCodeAsync(
                        sessionData: new SessionData(),
                        code: null,
                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
                });
        }
    }
}