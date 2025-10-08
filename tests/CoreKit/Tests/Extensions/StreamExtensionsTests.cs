// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Text;

using AppMotor.CoreKit.Extensions;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Extensions;

/// <summary>
/// Tests for <see cref="StreamExtensions"/>.
/// </summary>
public sealed class StreamExtensionsTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Test_ReadAsBytes(bool seekableStream)
    {
        // Setup
        var testFileInfo = new FileInfo("TestData/StreamReadTest.txt");
        testFileInfo.Exists.ShouldBe(true);

        using var fileStream = seekableStream ? new FileStream(testFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)
                                              : new NonSeekableFileStream(testFileInfo.FullName);

        // Test
        var bytes = fileStream.ReadAsBytes();

        // Verify
        bytes.Length.ShouldBe((int)testFileInfo.Length);
        bytes.ShouldBe(File.ReadAllBytes(testFileInfo.FullName));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Test_ReadAsBytesAsync(bool seekableStream)
    {
        // Setup
        var testFileInfo = new FileInfo("TestData/StreamReadTest.txt");
        testFileInfo.Exists.ShouldBe(true);

        await using var fileStream = seekableStream ? new FileStream(testFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)
                                                    : new NonSeekableFileStream(testFileInfo.FullName);

        // Test
        var bytes = await fileStream.ReadAsBytesAsync(TestContext.Current.CancellationToken);

        // Verify
        bytes.Length.ShouldBe((int)testFileInfo.Length);
        bytes.ShouldBe(await File.ReadAllBytesAsync(testFileInfo.FullName, TestContext.Current.CancellationToken));
    }

    [Fact]
    public void Test_ReadAsString()
    {
        // Setup
        var testFileInfo = new FileInfo("TestData/StreamReadTest.txt");
        testFileInfo.Exists.ShouldBe(true);

        using var fileStream = testFileInfo.OpenRead();

        // Test
        var contents = fileStream.ReadAsString(Encoding.UTF8);

        // Verify
        contents.ShouldBe(File.ReadAllText(testFileInfo.FullName, Encoding.UTF8));
    }

    [Fact]
    public async Task Test_ReadAsStringAsync()
    {
        // Setup
        var testFileInfo = new FileInfo("TestData/StreamReadTest.txt");
        testFileInfo.Exists.ShouldBe(true);

        await using var fileStream = testFileInfo.OpenRead();

        // Test
        var contents = await fileStream.ReadAsStringAsync(Encoding.UTF8, TestContext.Current.CancellationToken);

        // Verify
        // ReSharper disable once MethodHasAsyncOverload
        contents.ShouldBe(File.ReadAllText(testFileInfo.FullName, Encoding.UTF8));
    }

    private sealed class NonSeekableFileStream : FileStream
    {
        /// <inheritdoc />
        public override bool CanSeek => false;

        /// <inheritdoc />
        public override long Length => throw new NotSupportedException();

        /// <inheritdoc />
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public NonSeekableFileStream(string path)
            : base(path, FileMode.Open, FileAccess.Read, FileShare.Read)
        {
        }
    }
}
