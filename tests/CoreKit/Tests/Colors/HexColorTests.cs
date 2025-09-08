﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Drawing;

using AppMotor.CoreKit.Colors;
using AppMotor.TestKit;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Colors;

public sealed class HexColorTests : TestBase
{
    public HexColorTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Theory]
    [InlineData("#FFFF0000", 255, 255,   0,   0)] // red
    [InlineData("8000ff00",  128,   0, 255,   0)] // green
    [InlineData("#000F",       0,   0,   0, 255)] // blue
    [InlineData("f000",      255,   0,   0,   0)] // black
    [InlineData("FFF",       255, 255, 255, 255)] // white
    [InlineData("#ccc",      255, 204, 204, 204)] // light gray
    [InlineData("#ffff69B4", 255, 255, 105, 180)] // hot pink
    [InlineData("#80404060", 128,  64,  64,  96)] // dark blue
    [InlineData("#00734220",   0, 115,  66,  32)] // brown
    public void Test_Conversion(string hexString, byte a, byte r, byte g, byte b)
    {
        // Test
        var colorToTest = new HexColor(hexString);

        var rgbColor = colorToTest.ToRgb();

        // Verify
        colorToTest.A.ShouldBe(a);
        rgbColor.A.ShouldBe(a);
        rgbColor.R.ShouldBe(r);
        rgbColor.G.ShouldBe(g);
        rgbColor.B.ShouldBe(b);

        // Test 2
        var colorToTest2 = new HexColor(rgbColor);

        // Verify 2
        colorToTest2.ToRgb().ShouldBe(rgbColor);
        colorToTest2.HexString.ShouldBe(colorToTest.HexString);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("aa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaaa")]
    [InlineData("#XXX")]
    [InlineData("XXXXXXXX")]
    public void Test_InvalidHexStrings(string input)
    {
        var ex = Should.Throw<ArgumentException>(() => new HexColor(input));

        // For manual verification
        this.TestConsole.WriteLine(ex.Message);
    }

    [Theory]
    [InlineData("#FFFF0000", "#FFFF0000")] // red
    [InlineData("8000FF00",  "#8000FF00")] // green
    [InlineData("#000F",     "#000000FF")] // blue
    [InlineData("F000",      "#FF000000")] // black
    [InlineData("FFF",       "#FFFFFFFF")] // white
    [InlineData("#CCC",      "#FFCCCCCC")] // light gray
    [InlineData("#FFFF69B4", "#FFFF69B4")] // hot pink
    [InlineData("#80404060", "#80404060")] // dark blue
    [InlineData("#00734220", "#00734220")] // brown
    public void Test_HexStringAndToString(string input, string expectedHexString)
    {
        var colorFromUpperCase = new HexColor(input);
        colorFromUpperCase.HexString.ShouldBe(expectedHexString);
        colorFromUpperCase.ToString().ShouldBe(expectedHexString);

        var colorFromLowerCase = new HexColor(input.ToLowerInvariant());
        colorFromLowerCase.HexString.ShouldBe(expectedHexString);
        colorFromLowerCase.ToString().ShouldBe(expectedHexString);
    }

    [Fact]
    public void Test_Equals()
    {
        var red1 = new HexColor("#f00");
        var red2 = new HexColor(Color.Red);
        var green = new HexColor(Color.Green);

        EqualityMembersTests.TestEquals(red1, red2, green);
    }

    [Fact]
    public void Test_GetHashCode()
    {
        var red1 = new HexColor("#ff0000");
        var red2 = new HexColor(Color.Red);

        EqualityMembersTests.TestGetHashCode(red1, red2);
    }
}
