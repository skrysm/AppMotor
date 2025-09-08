// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CliKit.AppBuilding;
using AppMotor.CliKit.CommandLine;
using AppMotor.CoreKit.Certificates;
using AppMotor.CoreKit.Net;
using AppMotor.CoreKit.Net.Http;
using AppMotor.HttpServerKit.Startups;
using AppMotor.HttpServerKit.TestUtils;
using AppMotor.TestKit;
using AppMotor.TestKit.AppBuilding;
using AppMotor.TestKit.Networking;
using AppMotor.TestKit.Utils;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Shouldly;

using Xunit;

namespace AppMotor.HttpServerKit.Tests;

public sealed class HttpsTests : TestBase
{
    private const string SERVER_HOSTNAME = "localhost";

    public HttpsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task TestHttpsApiCall()
    {
        int testPort = ServerPortProvider.GetNextTestPort();

        using var testCertificate = TlsCertificate.CreateSelfSigned(SERVER_HOSTNAME, TimeSpan.FromDays(1));

        using var cts = new CancellationTokenSource();

        var app = new CliApplicationWithCommand(new TestServerCommand(testPort, testCertificate, this.TestConsole));
        Task appTask = app.RunAsync(cts.Token);

        using (var httpClient = HttpClientFactory.CreateHttpClient(serverCertificate: testCertificate))
        {
            // ReSharper disable once MethodSupportsCancellation
            var response = await httpClient.GetAsync($"https://{SERVER_HOSTNAME}:{testPort}/api/ping", TestContext.Current.CancellationToken);

            response.EnsureSuccessStatusCode();

            // ReSharper disable once MethodSupportsCancellation
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

            responseString.ShouldBe("Hello World!");
        }

        await cts.CancelAsync();

        await TestTimeout.TimeoutAfter(appTask, TimeSpan.FromSeconds(10));
    }

    private sealed class TestServerCommand : HttpServerCommandBase
    {
        private readonly int _port;

        private readonly TlsCertificate _testCertificate;

        private readonly DefaultHostBuilderFactory _hostBuilderFactory;

        public TestServerCommand(int port, TlsCertificate testCertificate, ITestOutputHelper testOutputHelper)
        {
            this._port = port;
            this._testCertificate = testCertificate;

            this._hostBuilderFactory = new XUnitHostBuilderFactory(testOutputHelper);
        }

        /// <inheritdoc />
        protected override IHostBuilder CreateHostBuilder()
        {
            return this._hostBuilderFactory.CreateHostBuilder();
        }

        /// <inheritdoc />
        protected override IEnumerable<HttpServerPort> GetServerPorts(IServiceProvider serviceProvider)
        {
            yield return new HttpsServerPort(
                SocketListenAddresses.Loopback,
                port: this._port,
                () => this._testCertificate,
                certificateProviderCallerOwnsCertificates: false
            );
        }

        /// <inheritdoc />
        protected override IAspNetStartup CreateStartupClass(WebHostBuilderContext context)
        {
            return new SimplePingStartup();
        }
    }
}
