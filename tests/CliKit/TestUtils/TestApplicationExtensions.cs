// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using Shouldly;

namespace AppMotor.CliKit.TestUtils;

internal static class TestApplicationExtensions
{
    public static void ShouldHaveNoOutput(this ITestApplication app)
    {
        app.TerminalOutput.ShouldBeEmpty();
    }
}
