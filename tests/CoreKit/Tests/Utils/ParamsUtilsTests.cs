// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Utils;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Utils;

public sealed class ParamsUtilsTests
{
    [Fact]
    public void TestCombineOne()
    {
        ParamsUtils.Combine(42, []).ShouldBe(new[] { 42 });
        ParamsUtils.Combine(42, [43]).ShouldBe(new[] { 42, 43 });
        ParamsUtils.Combine(42, [43, 44]).ShouldBe(new[] { 42, 43, 44 });
    }

    [Fact]
    public void TestCombineTwo()
    {
        ParamsUtils.Combine(42, 43, []).ShouldBe(new[] { 42, 43 });
        ParamsUtils.Combine(42, 43, [44]).ShouldBe(new[] { 42, 43, 44 });
        ParamsUtils.Combine(42, 43, [44, 45]).ShouldBe(new[] { 42, 43, 44, 45 });
    }
}
