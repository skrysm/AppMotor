// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Utils;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Utils;

/// <summary>
/// Tests for <see cref="LineTerminators"/> and <see cref="LineTerminatorsExtensions"/>.
/// </summary>
public sealed class LineTerminatorsTests
{
    [Fact]
    public void Test_AsString()
    {
        LineTerminators.SystemDefault.AsString().ShouldBe(Environment.NewLine);
        LineTerminators.CrLf.AsString().ShouldBe("\r\n");
        LineTerminators.Lf.AsString().ShouldBe("\n");
        Should.Throw<ArgumentOutOfRangeException>(() => ((LineTerminators)5000).AsString());
    }

    [Fact]
    public void Test_IsSystemDefault()
    {
        LineTerminators.SystemDefault.IsSystemDefault().ShouldBe(true);

        if (Environment.NewLine == "\r\n")
        {
            LineTerminators.CrLf.IsSystemDefault().ShouldBe(true);
            LineTerminators.Lf.IsSystemDefault().ShouldBe(false);
        }
        else
        {
            LineTerminators.CrLf.IsSystemDefault().ShouldBe(false);
            LineTerminators.Lf.IsSystemDefault().ShouldBe(true);
        }

        Should.Throw<ArgumentOutOfRangeException>(() => ((LineTerminators)5000).IsSystemDefault());
    }
}
