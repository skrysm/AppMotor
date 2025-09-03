﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Globalization;

using AppMotor.CoreKit.DateAndTime;
using AppMotor.CoreKit.Extensions;
using AppMotor.TestKit;
using AppMotor.TestKit.Shouldly;

using Newtonsoft.Json;

using Shouldly;

using Xunit;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AppMotor.CoreKit.Tests.DateAndTime;

/// <summary>
/// Tests for <see cref="DateTimeUtc"/>.
/// </summary>
public sealed class DateTimeUtcTests
{
    [Fact]
    public void Test_Now()
    {
        DateTimeUtc.Now.ShouldBe(DateTimeUtc.Now, tolerance: TimeSpan.FromMilliseconds(2));

        Thread.Sleep(TimeSpan.FromMilliseconds(500));

        // Check that is has changed
        DateTimeUtc.Now.ToDateTime().ShouldBe(DateTime.UtcNow, tolerance: TimeSpan.FromMilliseconds(2));
    }

    [Fact]
    public void Test_Constructor_DateTime_Utc()
    {
        var now = DateTime.UtcNow;
        var dateTimeUtc = new DateTimeUtc(now);

        dateTimeUtc.Year.ShouldBe(now.Year);
        dateTimeUtc.Month.ShouldBe(now.Month);
        dateTimeUtc.Day.ShouldBe(now.Day);
        dateTimeUtc.Hour.ShouldBe(now.Hour);
        dateTimeUtc.Minute.ShouldBe(now.Minute);
        dateTimeUtc.Second.ShouldBe(now.Second);
        dateTimeUtc.Millisecond.ShouldBe(now.Millisecond);

        dateTimeUtc.Ticks.ShouldBe(now.Ticks);
    }

    [Fact]
    public void Test_Constructor_DateTime_Local()
    {
        var now = DateTime.Now;
        var dateTimeUtc = new DateTimeUtc(now);

        dateTimeUtc.Ticks.ShouldBe(now.ToUniversalTime().Ticks);
    }

    [Fact]
    public void Test_Constructor_DateTime_Unspecified()
    {
        var dateTimeWithUnspecifiedKind = new DateTime(2020, 1, 2);
        Should.Throw<ArgumentException>(() => new DateTimeUtc(dateTimeWithUnspecifiedKind));
    }

    [Fact]
    public void Test_Constructor_DateTimeOffset()
    {
        var now = new DateTimeOffset(2020, 2, 3, 6, 5, 6, 7, offset: TimeSpan.FromHours(2));
        var dateTimeUtc = new DateTimeUtc(now);

        dateTimeUtc.Year.ShouldBe(2020);
        dateTimeUtc.Month.ShouldBe(2);
        dateTimeUtc.Day.ShouldBe(3);
        dateTimeUtc.Hour.ShouldBe(4);
        dateTimeUtc.Minute.ShouldBe(5);
        dateTimeUtc.Second.ShouldBe(6);
        dateTimeUtc.Millisecond.ShouldBe(7);
    }

    [Fact]
    public void Test_Constructor_Ticks()
    {
        var now = DateTime.UtcNow;
        var dateTimeUtc = new DateTimeUtc(now.Ticks);

        dateTimeUtc.Year.ShouldBe(now.Year);
        dateTimeUtc.Month.ShouldBe(now.Month);
        dateTimeUtc.Day.ShouldBe(now.Day);
        dateTimeUtc.Hour.ShouldBe(now.Hour);
        dateTimeUtc.Minute.ShouldBe(now.Minute);
        dateTimeUtc.Second.ShouldBe(now.Second);
        dateTimeUtc.Millisecond.ShouldBe(now.Millisecond);
        dateTimeUtc.Microsecond.ShouldBe(now.Microsecond);
        dateTimeUtc.Nanosecond.ShouldBe(now.Nanosecond);

        dateTimeUtc.Ticks.ShouldBe(now.Ticks);
    }

    [Fact]
    public void Test_Constructor_WithoutMilliseconds()
    {
        var dateTimeUtc = new DateTimeUtc(2020, 2, 3, 4, 5, 6);

        dateTimeUtc.Year.ShouldBe(2020);
        dateTimeUtc.Month.ShouldBe(2);
        dateTimeUtc.Day.ShouldBe(3);
        dateTimeUtc.Hour.ShouldBe(4);
        dateTimeUtc.Minute.ShouldBe(5);
        dateTimeUtc.Second.ShouldBe(6);
        dateTimeUtc.Millisecond.ShouldBe(0);
    }

    [Fact]
    public void Test_Constructor_WithMilliseconds()
    {
        var dateTimeUtc = new DateTimeUtc(2020, 2, 3, 4, 5, 6, 7);

        dateTimeUtc.Year.ShouldBe(2020);
        dateTimeUtc.Month.ShouldBe(2);
        dateTimeUtc.Day.ShouldBe(3);
        dateTimeUtc.Hour.ShouldBe(4);
        dateTimeUtc.Minute.ShouldBe(5);
        dateTimeUtc.Second.ShouldBe(6);
        dateTimeUtc.Millisecond.ShouldBe(7);
    }

    [Fact]
    public void Test_Date()
    {
        var dateTimeUtc = new DateTimeUtc(2020, 2, 3, 4, 5, 6, 7);
        var date = dateTimeUtc.Date;

        date.Year.ShouldBe(2020);
        date.Month.ShouldBe(2);
        date.Day.ShouldBe(3);
    }

    [Fact]
    public void Test_TimeOfDay()
    {
        var dateTimeUtc = new DateTimeUtc(2020, 2, 3, 4, 5, 6, 7);
        var timeOfDay = dateTimeUtc.TimeOfDay;

        timeOfDay.Hour.ShouldBe(4);
        timeOfDay.Minute.ShouldBe(5);
        timeOfDay.Second.ShouldBe(6);
        timeOfDay.Millisecond.ShouldBe(7);
    }

    [Fact]
    public void Test_DayOfWeek()
    {
        var dateTimeUtc = new DateTimeUtc(2021, 7, 1, 4, 5, 6);
        dateTimeUtc.DayOfWeek.ShouldBe(DayOfWeek.Thursday);
    }

    [Fact]
    public void Test_DayOfYear()
    {
        var today = DateTime.Today;
        var dateTimeUtc = new DateTimeUtc(today.Year, today.Month, today.Day, 4, 5, 6);
        dateTimeUtc.DayOfYear.ShouldBe(today.DayOfYear);
    }

    [Fact]
    public void Test_Microsecond()
    {
        // Setup
        var baseTime = new DateTimeUtc(2025, 9, 3, 12, 0, 0);

        // Test
        (baseTime + TimeSpan.FromTicks(20)).Microsecond.ShouldBe(2);
        (baseTime + TimeSpan.FromTicks(310)).Microsecond.ShouldBe(31);
        (baseTime + TimeSpan.FromTicks(9990)).Microsecond.ShouldBe(999);
    }

    [Fact]
    public void Test_Nanosecond()
    {
        // Setup
        var baseTime = new DateTimeUtc(2025, 9, 3, 12, 0, 0);

        // Test
        (baseTime + TimeSpan.FromTicks(1)).Nanosecond.ShouldBe(100);
        (baseTime + TimeSpan.FromTicks(5)).Nanosecond.ShouldBe(500);
        (baseTime + TimeSpan.FromTicks(9)).Nanosecond.ShouldBe(900);
    }

    [Fact]
    public void Test_HasFractionalSeconds_False()
    {
        // Setup
        var time = new DateTimeUtc(2025, 9, 3, 12, 31, 15);

        // Test
        time.HasFractionalSeconds.ShouldBe(false);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(9)]
    [InlineData(1_0)]
    [InlineData(1_1)]
    [InlineData(999_0)]
    [InlineData(5_000_0)]
    [InlineData(60_010_0)]
    [InlineData(123_000_3)]
    [InlineData(44_012_7)]
    public void Test_HasFractionalSeconds_True(int ticksToAdd)
    {
        // Setup
        var time = new DateTimeUtc(2025, 9, 3, 12, 31, 15) + TimeSpan.FromTicks(ticksToAdd);

        // Test
        time.HasFractionalSeconds.ShouldBe(true);
    }

    [Fact]
    public void Test_AddMonths()
    {
        var now = new DateTimeUtc(2021, 7, 1, 4, 5, 6);
        var later = now.AddMonths(2);
        later.Month.ShouldBe(9);
    }

    [Fact]
    public void Test_AddYears()
    {
        var now = new DateTimeUtc(2021, 7, 1, 4, 5, 6);
        var later = now.AddYears(3);
        later.Year.ShouldBe(2024);
    }

    [Fact]
    public void Test_ToDateTime()
    {
        // Test 1: source is DateTimeKind.Utc
        var source1 = DateTime.UtcNow;
        source1.Kind.ShouldBe(DateTimeKind.Utc); // verify assumption

        var dateTimeUtc1 = new DateTimeUtc(source1);

        dateTimeUtc1.ToDateTime().Ticks.ShouldBe(source1.Ticks);
        dateTimeUtc1.ToDateTime().Kind.ShouldBe(DateTimeKind.Utc);

        // Test 2: source is DateTimeKind.Local
        var source2 = source1.ToLocalTime();
        source2.Kind.ShouldBe(DateTimeKind.Local); // verify assumption

        var dateTimeUtc2 = new DateTimeUtc(source2);

        dateTimeUtc2.ToDateTime().Ticks.ShouldBe(source2.ToUniversalTime().Ticks);
        dateTimeUtc2.ToDateTime().Kind.ShouldBe(DateTimeKind.Utc);

        // Test 3: source is DateTimeOffset
        var source3 = new DateTimeOffset(source1.ToLocalTime());

        var dateTimeUtc3 = new DateTimeUtc(source3);

        dateTimeUtc3.ToDateTime().Ticks.ShouldBe(source3.ToUniversalTime().Ticks);
        dateTimeUtc3.ToDateTime().Kind.ShouldBe(DateTimeKind.Utc);
    }

    [Fact]
    public void Test_ToLocalTime()
    {
        // Test 1: source is DateTimeKind.Utc
        var source1 = DateTime.UtcNow;
        source1.Kind.ShouldBe(DateTimeKind.Utc); // verify assumption

        var dateTimeUtc1 = new DateTimeUtc(source1);

        dateTimeUtc1.ToLocalTime().Ticks.ShouldBe(source1.ToLocalTime().Ticks);
        dateTimeUtc1.ToLocalTime().Kind.ShouldBe(DateTimeKind.Local);

        // Test 2: source is DateTimeKind.Local
        var source2 = source1.ToLocalTime();
        source2.Kind.ShouldBe(DateTimeKind.Local); // verify assumption

        var dateTimeUtc2 = new DateTimeUtc(source2);

        dateTimeUtc2.ToLocalTime().Ticks.ShouldBe(source2.Ticks);
        dateTimeUtc2.ToLocalTime().Kind.ShouldBe(DateTimeKind.Local);

        // Test 3: source is DateTimeOffset
        var source3 = new DateTimeOffset(source1.ToLocalTime());

        var dateTimeUtc3 = new DateTimeUtc(source3);

        dateTimeUtc3.ToLocalTime().Ticks.ShouldBe(source3.ToLocalTime().Ticks);
        dateTimeUtc3.ToLocalTime().Kind.ShouldBe(DateTimeKind.Local);
    }

    [Fact]
    public void Test_ToDateTimeOffset()
    {
        var now = DateTime.UtcNow;
        var dateTimeUtc = new DateTimeUtc(now);

        var dateTimeOffset = dateTimeUtc.ToDateTimeOffset();
        dateTimeOffset.Offset.ShouldBe(TimeSpan.Zero);
        dateTimeOffset.Ticks.ShouldBe(now.Ticks);
    }

    [Fact]
    public void Test_ImplicitConversion_DateTime()
    {
        var now = DateTime.UtcNow;

        DateTimeUtc nowAsUtc = now;
        nowAsUtc.Ticks.ShouldBe(now.Ticks);

        DateTime now2 = nowAsUtc;
        now2.Ticks.ShouldBe(now.Ticks);
    }

    [Fact]
    public void Test_ImplicitConversion_DateTimeOffset()
    {
        var now = DateTimeOffset.UtcNow;

        DateTimeUtc nowAsUtc = now;
        nowAsUtc.Ticks.ShouldBe(now.Ticks);

        DateTimeOffset now2 = nowAsUtc;
        now2.Ticks.ShouldBe(now.Ticks);
    }

    [Fact]
    public void Test_Add_TimeSpan()
    {
        var now = new DateTimeUtc(2020, 2, 3, 4, 5, 6, 7);
        DateTimeUtc later = now + TimeSpan.FromHours(10);

        later.Year.ShouldBe(2020);
        later.Month.ShouldBe(2);
        later.Day.ShouldBe(3);
        later.Hour.ShouldBe(14);
        later.Minute.ShouldBe(5);
        later.Second.ShouldBe(6);
        later.Millisecond.ShouldBe(7);
    }

    [Fact]
    public void Test_Subtract_TimeSpan()
    {
        var now = new DateTimeUtc(2020, 2, 3, 14, 5, 6, 7);
        DateTimeUtc later = now - TimeSpan.FromHours(10);

        later.Year.ShouldBe(2020);
        later.Month.ShouldBe(2);
        later.Day.ShouldBe(3);
        later.Hour.ShouldBe(4);
        later.Minute.ShouldBe(5);
        later.Second.ShouldBe(6);
        later.Millisecond.ShouldBe(7);
    }

    [Fact]
    public void Test_Subtract_DateTimeUtc()
    {
        var a = new DateTimeUtc(2020, 2, 3, 14, 5, 6, 7);
        var b = new DateTimeUtc(2020, 2, 3, 4, 5, 6, 7);

        var diff = a - b;

        diff.TotalHours.ShouldBe(10);
    }

    [Fact]
    public void Test_ToString()
    {
        var now = DateTime.UtcNow;
        var dateTimeUtc = new DateTimeUtc(now);

        // ReSharper disable once SpecifyACultureInStringConversionExplicitly
        dateTimeUtc.ToString().ShouldBe(now.ToString());
        dateTimeUtc.ToString(null).ShouldBe(now.ToString((string?)null));
        dateTimeUtc.ToString(null, null).ShouldBe(now.ToString(null, null));
        dateTimeUtc.ToString("o").ShouldBe(now.ToStringIC("yyyy-MM-ddTHH:mm:ss.fffffffZ"));
    }

    [Fact]
    public void Test_Equals()
    {
        var now = DateTimeUtc.Now;
        var later = now + TimeSpan.FromMilliseconds(1);

        EqualityMembersTests.TestEquals(now, later);
    }

    [Fact]
    public void Test_GetHashCode()
    {
        var nowDateTime = DateTime.Now;

        var a = new DateTimeUtc(nowDateTime);
        var b = new DateTimeUtc(nowDateTime.ToUniversalTime());

        EqualityMembersTests.TestGetHashCode(a, b);
    }

    [Fact]
    public void Test_Compare()
    {
        var now = DateTimeUtc.Now;
        var later = now + TimeSpan.FromMilliseconds(1);

        now.CompareTo(now).ShouldBe(0);
        now.CompareTo(later).ShouldBe(-1);
        later.CompareTo(now).ShouldBe(1);

        now.CompareTo((object)now).ShouldBe(0);
        now.CompareTo((object)later).ShouldBe(-1);
        later.CompareTo((object)now).ShouldBe(1);
        Should.Throw<ArgumentException>(() => now.CompareTo(new object()));
        now.CompareTo(null).ShouldBe(1);

        (now < later).ShouldBe(true);
        (later < now).ShouldBe(false);
        (now > later).ShouldBe(false);
        (later > now).ShouldBe(true);

        (now <= later).ShouldBe(true);
        (later <= now).ShouldBe(false);
        (now <= later - TimeSpan.FromMilliseconds(1)).ShouldBe(true);
        (now >= later).ShouldBe(false);
        (later >= now).ShouldBe(true);
        (later - TimeSpan.FromMilliseconds(1) >= now).ShouldBe(true);
    }

    [Fact]
    public void Test_JsonSerialization_SystemTextJson()
    {
        var testData = new JsonTestData(new DateTimeUtc(2020, 2, 3, 4, 5, 6, 7));

        var json = JsonSerializer.Serialize(testData);

        json.ShouldBe("{\"SomeTime\":\"2020-02-03T04:05:06.007Z\"}");

        var deserializedTestData = JsonSerializer.Deserialize<JsonTestData>(json);

        deserializedTestData!.SomeTime.ShouldBe(testData.SomeTime);
    }

    [Fact]
    public void Test_JsonSerialization_NewtonsoftJson()
    {
        var testData = new JsonTestData(new DateTimeUtc(2020, 2, 3, 4, 5, 6, 7));

        var json = JsonConvert.SerializeObject(testData);

        json.ShouldBe("{\"SomeTime\":\"2020-02-03T04:05:06.007Z\"}");

        var deserializedTestData = JsonConvert.DeserializeObject<JsonTestData>(json);

        deserializedTestData!.SomeTime.ShouldBe(testData.SomeTime);
    }

    [Fact]
    public void Test_JsonSerialization_NonIso8601_SystemTextJson()
    {
        const string JSON = "{\"SomeTime\":\"6/15/2009 1:45:30 PM\"}";

        var ex = Should.Throw<System.Text.Json.JsonException>(() => JsonSerializer.Deserialize<JsonTestData>(JSON));

        ex.Message.ShouldContain("Path: $.SomeTime");
    }

    [Fact]
    public void Test_JsonSerialization_NonIso8601_NewtonsoftJson()
    {
        const string JSON = "{\"SomeTime\":\"6/15/2009 1:45:30 PM\"}";

        var ex = Should.Throw<JsonSerializationException>(() => JsonConvert.DeserializeObject<JsonTestData>(JSON));

        ex.Message.ShouldContain("Path 'SomeTime'");
    }

    private record JsonTestData(DateTimeUtc SomeTime);

    [Fact]
    public void Test_JsonSerialization_Null_SystemTextJson()
    {
        var testData = new JsonTestDataNullable(null);

        var json = JsonSerializer.Serialize(testData);

        json.ShouldBe("{\"SomeTime\":null}");

        var deserializedTestData = JsonSerializer.Deserialize<JsonTestDataNullable>(json);

        deserializedTestData!.SomeTime.ShouldBe(null);
    }

    [Fact]
    public void Test_JsonSerialization_Null_NewtonsoftJson()
    {
        var testData = new JsonTestDataNullable(null);

        var json = JsonConvert.SerializeObject(testData);

        json.ShouldBe("{\"SomeTime\":null}");

        var deserializedTestData = JsonConvert.DeserializeObject<JsonTestDataNullable>(json);

        deserializedTestData!.SomeTime.ShouldBe(null);
    }

    private record JsonTestDataNullable(DateTimeUtc? SomeTime);

    [Theory]
    [InlineData("09/15/07 08:45:00 +1:00", "2007-09-15T07:45:00")]
    [InlineData("2007-09-15T07:45:00Z", "2007-09-15T07:45:00")]
    [InlineData("abc", null)]
    public void Test_Parse(string input, string? expectedIsoString)
    {
        if (expectedIsoString is not null)
        {
            DateTimeUtc.Parse(input, CultureInfo.InvariantCulture).ToString("s", CultureInfo.InvariantCulture).ShouldBe(expectedIsoString);
        }
        else
        {
            var formatException = Should.Throw<FormatException>(() => DateTimeUtc.Parse(input, CultureInfo.InvariantCulture));
            formatException.Data["InputString"].ShouldBe(input);
        }
    }

    [Theory]
    [InlineData("09/15/07 08:45:00 +1:00", "2007-09-15T07:45:00")]
    [InlineData("2007-09-15T07:45:00Z", "2007-09-15T07:45:00")]
    [InlineData("abc", null)]
    public void Test_TryParse(string input, string? expectedIsoString)
    {
        bool success = DateTimeUtc.TryParse(input, CultureInfo.InvariantCulture, out var result);

        if (expectedIsoString is not null)
        {
            success.ShouldBe(true);
            result.ToString("s", CultureInfo.InvariantCulture).ShouldBe(expectedIsoString);
        }
        else
        {
            success.ShouldBe(false);
        }
    }

    [Theory]
    [InlineData("09/15/07 08:45:00 +1:00", null)]
    [InlineData("2007-09-15T07:45:00Z", "2007-09-15T07:45:00")]
    [InlineData("abc", null)]
    public void Test_ParseExact_OneFormat(string input, string? expectedIsoString)
    {
        DateTimeUtc FunctionUnderTest()
        {
            return DateTimeUtc.ParseExact(input, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        if (expectedIsoString is not null)
        {
            FunctionUnderTest().ToString("s", CultureInfo.InvariantCulture).ShouldBe(expectedIsoString);
        }
        else
        {
            var formatException = Should.Throw<FormatException>(() => FunctionUnderTest());
            formatException.Data["InputString"].ShouldBe(input);
        }
    }

    [Theory]
    [InlineData("09/15/07 08:45:00 +1:00", "2007-09-15T07:45:00")]
    [InlineData("2007-09-15T07:45:00Z", "2007-09-15T07:45:00")]
    [InlineData("abc", null)]
    public void Test_ParseExact_MultipleFormats(string input, string? expectedIsoString)
    {
        DateTimeUtc FunctionUnderTest()
        {
            return DateTimeUtc.ParseExact(input, ["yyyy-MM-ddTHH:mm:ssZ", "MM/dd/yy HH:mm:ss K"], CultureInfo.InvariantCulture);
        }

        if (expectedIsoString is not null)
        {
            FunctionUnderTest().ToString("s", CultureInfo.InvariantCulture).ShouldBe(expectedIsoString);
        }
        else
        {
            var formatException = Should.Throw<FormatException>(() => FunctionUnderTest());
            formatException.Data["InputString"].ShouldBe(input);
        }
    }

    [Theory]
    [InlineData("09/15/07 08:45:00 +1:00", null)]
    [InlineData("2007-09-15T07:45:00Z", "2007-09-15T07:45:00")]
    [InlineData("abc", null)]
    public void Test_TryParseExact_OneFormat(string input, string? expectedIsoString)
    {
        bool success = DateTimeUtc.TryParseExact(input, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, out var result);

        if (expectedIsoString is not null)
        {
            success.ShouldBe(true);
            result.ToString("s", CultureInfo.InvariantCulture).ShouldBe(expectedIsoString);
        }
        else
        {
            success.ShouldBe(false);
        }
    }

    [Theory]
    [InlineData("09/15/07 08:45:00 +1:00", "2007-09-15T07:45:00")]
    [InlineData("2007-09-15T07:45:00Z", "2007-09-15T07:45:00")]
    [InlineData("abc", null)]
    public void Test_TryParseExact_MultipleFormats(string input, string? expectedIsoString)
    {
        bool success = DateTimeUtc.TryParseExact(input, ["yyyy-MM-ddTHH:mm:ssZ", "MM/dd/yy HH:mm:ss K"], CultureInfo.InvariantCulture, out var result);

        if (expectedIsoString is not null)
        {
            success.ShouldBe(true);
            result.ToString("s", CultureInfo.InvariantCulture).ShouldBe(expectedIsoString);
        }
        else
        {
            success.ShouldBe(false);
        }
    }
}
