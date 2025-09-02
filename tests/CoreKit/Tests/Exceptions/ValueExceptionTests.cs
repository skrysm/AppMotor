// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Exceptions;
using AppMotor.CoreKit.Utils;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Exceptions;

public sealed class ValueExceptionTests
{
    [Fact]
    public void TestDefaultConstructor()
    {
        var ex = new ValueException();

        ex.ValueName.ShouldBe(null);
        ex.Message.ShouldBe(Validate.ExceptionMessages.DEFAULT_MESSAGE);
    }
}
