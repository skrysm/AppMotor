// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Resources;

using AppMotor.CoreKit.Extensions;
using AppMotor.CoreKit.TestData;
using AppMotor.CoreKit.Utils;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Utils;

/// <summary>
/// Tests for <see cref="AssemblyEmbeddedResources"/>.
/// </summary>
public sealed class AssemblyEmbeddedResourcesTests
{
    [Fact]
    public void Test_GetResource_NameOnly()
    {
        // Setup
        var embeddedResources = GetType().Assembly.GetEmbeddedResources();

        // Tests
        using var existingStream = embeddedResources.GetResource("TestData/StreamReadTest.txt");
        existingStream.ShouldNotBeNull();

        using var nonExistingStream = embeddedResources.GetResource("I-do-not-exist.txt");
        nonExistingStream.ShouldBeNull();
    }

    [Fact]
    public void Test_GetResource_NameAndType()
    {
        // Setup
        var embeddedResources = GetType().Assembly.GetEmbeddedResources();

        // Tests
        using var existingStream = embeddedResources.GetResource(typeof(ResourceLocatorType), "AnotherFile.txt");
        existingStream.ShouldNotBeNull();

        using var nonExistingStream = embeddedResources.GetResource(typeof(ResourceLocatorType), "I-do-not-exist.txt");
        nonExistingStream.ShouldBeNull();
    }

    [Fact]
    public void Test_GetRequiredResource_NameOnly()
    {
        // Setup
        var embeddedResources = GetType().Assembly.GetEmbeddedResources();

        // Tests
        using var existingStream = embeddedResources.GetRequiredResource("TestData/StreamReadTest.txt");
        existingStream.ShouldNotBeNull();

        var ex = Should.Throw<MissingManifestResourceException>(() => embeddedResources.GetRequiredResource("I-do-not-exist.txt"));

        // Verify
        ex.Message.ShouldBe("The embedded resource 'I-do-not-exist.txt' was not found in assembly 'AppMotor.CoreKit.Tests'.");
    }

    [Fact]
    public void Test_GetRequiredResource_NameAndType()
    {
        // Setup
        var embeddedResources = GetType().Assembly.GetEmbeddedResources();

        // Tests
        using var existingStream = embeddedResources.GetRequiredResource(typeof(ResourceLocatorType), "AnotherFile.txt");
        existingStream.ShouldNotBeNull();

        var ex = Should.Throw<MissingManifestResourceException>(() => embeddedResources.GetRequiredResource(typeof(ResourceLocatorType), "I-do-not-exist.txt"));

        // Verify
        ex.Message.ShouldBe("The embedded resource 'AppMotor.CoreKit.TestData.I-do-not-exist.txt' was not found in assembly 'AppMotor.CoreKit.Tests'.");
    }
}
