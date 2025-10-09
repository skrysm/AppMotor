// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Reflection;
using System.Resources;

using AppMotor.CoreKit.Extensions;
using AppMotor.CoreKit.TestData;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Extensions;

/// <summary>
/// Tests for <see cref="CoreKit.Extensions.AssemblyExtensions"/>.
/// </summary>
public sealed class AssemblyExtensionsTests
{
    private static Assembly ThisAssembly => Assembly.GetExecutingAssembly();

    [Fact]
    public void Test_GetSimpleName()
    {
        ThisAssembly.GetSimpleName().ShouldBe("AppMotor.CoreKit.Tests");
    }

    [Fact]
    public void Test_DoesEmbeddedResourceExist()
    {
        ThisAssembly.DoesEmbeddedResourceExist("TestData/StreamReadTest.txt").ShouldBe(true);
        ThisAssembly.DoesEmbeddedResourceExist("I-do-not-exist.txt").ShouldBe(false);
    }

    [Fact]
    public void Test_GetEmbeddedResource_NameOnly()
    {
        Should.NotThrow(() => ThisAssembly.GetEmbeddedResource("TestData/StreamReadTest.txt"));

        var ex = Should.Throw<MissingManifestResourceException>(() => ThisAssembly.GetEmbeddedResource("I-do-not-exist.txt"));
        ex.Message.ShouldBe("The embedded resource 'I-do-not-exist.txt' was not found in assembly 'AppMotor.CoreKit.Tests'.");
    }

    [Fact]
    public void Test_GetEmbeddedResource_TypeAndName()
    {
        Should.NotThrow(() => ThisAssembly.GetEmbeddedResource(typeof(ResourceLocatorType), "AnotherFile.txt"));

        var ex = Should.Throw<MissingManifestResourceException>(() => ThisAssembly.GetEmbeddedResource(typeof(ResourceLocatorType), "I-do-not-exist.txt"));
        ex.Message.ShouldBe("The embedded resource 'AppMotor.CoreKit.TestData.I-do-not-exist.txt' was not found in assembly 'AppMotor.CoreKit.Tests'.");
    }

    [Fact]
    public void Test_GetEmbeddedResourceOrNull_NameOnly()
    {
        ThisAssembly.GetEmbeddedResourceOrNull("TestData/StreamReadTest.txt").ShouldNotBeNull();
        ThisAssembly.GetEmbeddedResourceOrNull("I-do-not-exist.txt").ShouldBe(null);
    }

    [Fact]
    public void Test_GetEmbeddedResourceOrNull_TypeAndName()
    {
        ThisAssembly.GetEmbeddedResourceOrNull(typeof(ResourceLocatorType), "AnotherFile.txt").ShouldNotBeNull();
        ThisAssembly.GetEmbeddedResourceOrNull(typeof(ResourceLocatorType), "I-do-not-exist.txt").ShouldBe(null);
    }
}
