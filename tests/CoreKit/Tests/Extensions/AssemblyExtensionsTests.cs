// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Reflection;

using AppMotor.CoreKit.Extensions;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Extensions;

/// <summary>
/// Tests for <see cref="CoreKit.Extensions.AssemblyExtensions"/>.
/// </summary>
public sealed class AssemblyExtensionsTests
{
    [Fact]
    public void Test_GetSimpleName()
    {
        Assembly.GetExecutingAssembly().GetSimpleName().ShouldBe("AppMotor.CoreKit.Tests");
    }
}
