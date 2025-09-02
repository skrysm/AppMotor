// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using JetBrains.Annotations;

namespace AppMotor.CliApp.TestUtils;

[PublicAPI]
internal interface ITestApplication
{
    string TerminalOutput { get; }
}
