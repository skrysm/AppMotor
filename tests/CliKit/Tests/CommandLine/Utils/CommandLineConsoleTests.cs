// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CliKit.CommandLine.Utils;
using AppMotor.CliKit.Terminals;

using Shouldly;

using Xunit;

namespace AppMotor.CliKit.Tests.CommandLine.Utils;

/// <summary>
/// Tests for <see cref="CommandLineConsole"/>.
/// </summary>
public sealed class CommandLineConsoleTests
{
    [Fact]
    public void Test_FromTerminal_RealTerminal()
    {
        CommandLineConsole.FromTerminal(Terminal.Instance).ShouldBe(null);
    }
}
