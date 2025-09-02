// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Utils;
using AppMotor.TestKit.Extensions;

using Shouldly;

using Xunit;

namespace AppMotor.CoreKit.Tests.Utils;

public sealed class TaskUtilsTests
{
    [Fact]
    public async Task Test_DelaySafe_NotCanceled()
    {
        var result = await TaskUtils.DelaySafe(TimeSpan.FromSeconds(0.2), CancellationToken.None).OrTimeoutAfter(TimeSpan.FromSeconds(2));
        result.ShouldBe(true);
    }

    [Fact]
    public async Task Test_DelaySafe_Canceled()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.2));

        var result = await TaskUtils.DelaySafe(TimeSpan.FromSeconds(20), cts.Token).OrTimeoutAfter(TimeSpan.FromSeconds(2));
        result.ShouldBe(false);
    }
}
