// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

namespace AppMotor.CliKit.TestUtils;

internal class TestCliApplication : TestCliApplicationBase
{
    /// <inheritdoc />
    protected override CliApplicationExecutor MainExecutor { get; }

    /// <inheritdoc />
    public TestCliApplication(Action mainAction)
    {
        this.MainExecutor = new(mainAction);
    }
}
