// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CliKit.CommandLine;

namespace AppMotor.CliKit.TestUtils;

internal class TestApplicationWithVerbs : TestApplicationWithVerbsBase
{
    public TestApplicationWithVerbs(params CliVerb[] verbs)
        : base(verbs)
    {
    }
}
