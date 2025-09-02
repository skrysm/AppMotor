﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CliApp.CommandLine;

namespace AppMotor.CliApp.TestUtils;

internal class TestApplicationWithVerbs : TestApplicationWithVerbsBase
{
    public TestApplicationWithVerbs(params CliVerb[] verbs)
        : base(verbs)
    {
    }
}
