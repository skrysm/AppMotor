// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using Microsoft.Extensions.Logging;

using Xunit;

namespace AppMotor.TestKit.Logging;

/// <summary>
/// Logger provider for <see cref="XUnitLogger"/>.
/// </summary>
internal sealed class XUnitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly TestLoggerStatistics _loggerStatistics;

    public XUnitLoggerProvider(ITestOutputHelper testOutputHelper, TestLoggerStatistics loggerStatistics)
    {
        this._testOutputHelper = testOutputHelper;
        this._loggerStatistics = loggerStatistics;
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return new XUnitLogger(this._testOutputHelper, categoryName, this._loggerStatistics);
    }
}
