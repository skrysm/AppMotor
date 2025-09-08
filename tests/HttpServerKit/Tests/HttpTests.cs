// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CliKit.AppBuilding;
using AppMotor.CliKit.CommandLine;
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

public sealed class HttpTests : TestBase
{
    public HttpTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task TestHttpApiCall()
    {
        int testPort = ServerPortProvider.GetNextTestPort();

        using var cts = new CancellationTokenSource();

        var app = new CliApplicationWithCommand(new TestHttpServerCommand(testPort, this.TestConsole));
        Task appTask = app.RunAsync(cts.Token);

        using (var httpClient = HttpClientFactory.CreateHttpClient())
        {
            // ReSharper disable once MethodSupportsCancellation
            var response = await httpClient.GetAsync($"http://localhost:{testPort}/api/ping", TestContext.Current.CancellationToken);

            response.EnsureSuccessStatusCode();

            // ReSharper disable once MethodSupportsCancellation
            var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

            responseString.ShouldBe("Hello World!");
        }

        await cts.CancelAsync();

        await TestTimeout.TimeoutAfter(appTask, TimeSpan.FromSeconds(10));
    }

    private sealed class TestHttpServerCommand : HttpServerCommandBase
    {
        private readonly int _testPort;

        private readonly DefaultHostBuilderFactory _hostBuilderFactory;

        public TestHttpServerCommand(int testPort, ITestOutputHelper testOutputHelper)
        {
            this._testPort = testPort;

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
            yield return new HttpServerPort(SocketListenAddresses.Loopback, this._testPort);
        }

        /// <inheritdoc />
        protected override IAspNetStartup CreateStartupClass(WebHostBuilderContext context)
        {
            return new SimplePingStartup();
        }
    }
}
