// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CliKit.CommandLine;

namespace AppMotor.CliKit.TestUtils;

internal class TestVerbGroup : CliVerb
{
    public TestVerbGroup(string name, params CliVerb[] subVerbs) : base(name)
    {
        this.SubVerbs = subVerbs;
    }
}
