// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Extensions;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Extensions;

/// <summary>
/// Tests for <see cref="DirectoryInfoExtensions"/>.
/// </summary>
public sealed class DirectoryInfoExtensionsTests
{
    [Fact]
    public void Test_GetFile()
    {
        // Setup
        var parent = new DirectoryInfo(Path.GetTempPath());

        // Test
        FileInfo child = parent.GetFile("my-sub-dir");

        // Verify
        child.ShouldNotBeNull();
        child.Name.ShouldBe("my-sub-dir");
        child.FullName.ShouldBe(Path.Combine(Path.GetTempPath(), "my-sub-dir"));
    }

    [Fact]
    public void Test_GetDirectory()
    {
        // Setup
        var parent = new DirectoryInfo(Path.GetTempPath());

        // Test
        DirectoryInfo child = parent.GetDirectory("my-sub-dir");

        // Verify
        child.ShouldNotBeNull();
        child.Name.ShouldBe("my-sub-dir");
        child.FullName.ShouldBe(Path.Combine(Path.GetTempPath(), "my-sub-dir"));
    }
}
