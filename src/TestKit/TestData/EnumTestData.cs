﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Utils;

namespace AppMotor.TestKit.TestData;

/// <summary>
/// Provides all members/values of an enum as test data to be used as <c>[ClassData(typeof(EnumTestData&lt;MyEnum&gt;))]</c>.
/// </summary>
public sealed class EnumTestData<TEnum> : TestDataBase where TEnum : Enum
{
    /// <inheritdoc />
    public override IEnumerator<object[]> GetEnumerator()
    {
        foreach (var enumValue in EnumUtils.GetValues<TEnum>())
        {
            yield return [enumValue];
        }
    }
}
