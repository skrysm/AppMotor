// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Reflection;
using System.Text;

using AppMotor.CoreKit.Extensions;
using AppMotor.CoreKit.Utils;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Utils;

/// <summary>
/// Tests for <see cref="EmbeddedResource"/>.
/// </summary>
public sealed class EmbeddedResourceTests
{
    private static Assembly ThisAssembly => Assembly.GetExecutingAssembly();

    [Fact]
    public void Test_AsStream()
    {
        // Setup
        var testFileInfo = new FileInfo("TestData/StreamReadTest.txt");
        testFileInfo.Exists.ShouldBe(true);

        var embeddedResource = ThisAssembly.GetEmbeddedResource("TestData/StreamReadTest.txt");

        // Tests
        using var stream = embeddedResource.AsStream();
        var bytes = stream.ReadAsBytes();

        // Verify
        bytes.ShouldBe(File.ReadAllBytes(testFileInfo.FullName));
    }

    [Fact]
    public void Test_AsBytes()
    {
        // Setup
        var testFileInfo = new FileInfo("TestData/StreamReadTest.txt");
        testFileInfo.Exists.ShouldBe(true);

        var embeddedResource = ThisAssembly.GetEmbeddedResource("TestData/StreamReadTest.txt");

        // Tests
        var bytes = embeddedResource.AsBytes();

        // Verify
        bytes.ShouldBe(File.ReadAllBytes(testFileInfo.FullName));
    }

    [Fact]
    public void Test_AsString()
    {
        // Setup
        var testFileInfo = new FileInfo("TestData/StreamReadTest.txt");
        testFileInfo.Exists.ShouldBe(true);

        var embeddedResource = ThisAssembly.GetEmbeddedResource("TestData/StreamReadTest.txt");

        // Tests
        var contents = embeddedResource.AsString(Encoding.UTF8);

        // Verify
        contents.ShouldBe(File.ReadAllText(testFileInfo.FullName, Encoding.UTF8));
    }

    [Fact]
    public void Test_AsLines()
    {
        // Setup
        var testFileInfo = new FileInfo("TestData/StreamReadTest.txt");
        testFileInfo.Exists.ShouldBe(true);

        var embeddedResource = ThisAssembly.GetEmbeddedResource("TestData/StreamReadTest.txt");

        // Tests
        var contents = embeddedResource.AsLines(Encoding.UTF8).ToArray();

        // Verify
        contents.ShouldBe(File.ReadAllLines(testFileInfo.FullName, Encoding.UTF8));
    }
}
