﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Drawing;

using AppMotor.CoreKit.Colors;
using AppMotor.CoreKit.Extensions;
using AppMotor.TestKit;
using AppMotor.TestKit.TestData;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Colors;

public sealed class HslColorTests
{
    // For expected values, see: https://convertacolor.com/
    [Theory]
    [InlineData(255,   0,   0,   0.0, 100.0,  50.0)] // red
    [InlineData(  0, 255,   0, 120.0, 100.0,  50.0)] // green
    [InlineData(  0,   0, 255, 240.0, 100.0,  50.0)] // blue
    [InlineData(  0,   0,   0,   0.0,   0.0,   0.0)] // black
    [InlineData(255, 255, 255,   0.0,   0.0, 100.0)] // white
    [InlineData(128, 128, 128,   0.0,   0.0,  50.2)] // gray
    [InlineData(255, 105, 180, 330.0, 100.0,  70.6)] // hot pink
    [InlineData( 64,  64,  96, 240.0,  20.0,  31.4)] // dark blue
    [InlineData(115,  66,  32,  24.6,  56.5,  28.8)] // brown
    public void Test_Conversion(byte r, byte g, byte b, double expectedH, double expectedS, double expectedL)
    {
        // Setup
        var rgbColor = new RgbColor(a: 128, r, g, b);

        // Test
        var colorToTest = new HslColor(rgbColor);

        // Verify
        colorToTest.A.ShouldBe((byte)128);
        Math.Round(colorToTest.H, digits: 1).ShouldBe(expectedH);
        Math.Round(colorToTest.S, digits: 1).ShouldBe(expectedS);
        Math.Round(colorToTest.L, digits: 1).ShouldBe(expectedL);

        var convertedBack = colorToTest.ToRgb();

        convertedBack.ShouldBe(rgbColor);
    }

    [Theory]
    [InlineData(  0, 100.0,  50.0,   0, 100.0, 100.0)] // red
    [InlineData(120, 100.0,  50.0, 120, 100.0, 100.0)] // green
    [InlineData(240, 100.0,  50.0, 240, 100.0, 100.0)] // blue
    [InlineData(  0,   0.0,   0.0,   0,   0.0,   0.0)] // black
    [InlineData(  0,   0.0, 100.0,   0,   0.0, 100.0)] // white
    [InlineData(  0,   0.0,  50.2,   0,   0.0,  50.2)] // gray
    [InlineData(330, 100.0,  70.6, 330,  58.8, 100.0)] // hot pink
    [InlineData(240,  20.0,  31.4, 240,  33.3,  37.7)] // dark blue
    [InlineData(24.6, 56.5,  28.8, 24.6, 72.2,  45.1)] // brown
    public void Test_ToHsv(float h, float s, float l, double expectedH2, double expectedS2, double expectedV)
    {
        var hsvColor = new HslColor(a: 128, h, s, l).ToHsv();

        hsvColor.A.ShouldBe((byte)128);
        Math.Round(hsvColor.H, digits: 1).ShouldBe(expectedH2);
        Math.Round(hsvColor.S, digits: 1).ShouldBe(expectedS2);
        Math.Round(hsvColor.V, digits: 1).ShouldBe(expectedV);
    }

    [Theory]
    [ClassData(typeof(EnumTestData<KnownColor>))]
    public void Test_KnownColors(KnownColor knownColor)
    {
        var rgbColor = new RgbColor(knownColor);

        var hslColor = rgbColor.ToHsl();

        hslColor.A.ShouldBe(rgbColor.A);

        hslColor.ToRgb().ShouldBe(rgbColor);
    }

    [Theory]
    [InlineData(12, 24, 48)]
    public void Test_ToString(float h, float s, float l)
    {
        new HslColor(h, s, l).ToString().ShouldBe($"{nameof(HslColor)} [A=255, H={h}, S={s}, L={l}]");
        new HslColor(a: 128, h, s, l).ToString().ShouldBe($"{nameof(HslColor)} [A=128, H={h}, S={s}, L={l}]");
    }

    [Fact]
    public void Test_Equals()
    {
        var red1 = new HslColor(0, 100, 50);
        var red2 = new HslColor(RgbColor.Red);
        var green = new HslColor(RgbColor.Green);

        EqualityMembersTests.TestEquals(red1, red2, green);
    }

    [Fact]
    public void Test_GetHashCode()
    {
        var red1 = new HslColor(0, 100, 50);
        var red2 = new HslColor(RgbColor.Red);

        EqualityMembersTests.TestGetHashCode(red1, red2);
    }

    [Fact]
    public void Test_InvalidConstructorParameters()
    {
        Should.Throw<ArgumentOutOfRangeException>(() => new HslColor(-1, 50, 50));
        Should.Throw<ArgumentOutOfRangeException>(() => new HslColor(360, 50, 50));

        Should.Throw<ArgumentOutOfRangeException>(() => new HslColor(100, -1, 50));
        Should.Throw<ArgumentOutOfRangeException>(() => new HslColor(100, 101, 50));

        Should.Throw<ArgumentOutOfRangeException>(() => new HslColor(100, 50, -1));
        Should.Throw<ArgumentOutOfRangeException>(() => new HslColor(100, 50, 101));
    }
}
