﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.CommandLine;
using System.CommandLine.IO;
using System.Diagnostics.CodeAnalysis;

using AppMotor.CliKit.Terminals;

namespace AppMotor.CliKit.CommandLine.Utils;

internal sealed class CommandLineConsole : IConsole
{
    private readonly ITerminal _terminal;

    /// <inheritdoc />
    public IStandardStreamWriter Out { get; }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public bool IsOutputRedirected => this._terminal.IsOutputRedirected;

    /// <inheritdoc />
    public IStandardStreamWriter Error { get; }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public bool IsErrorRedirected => this._terminal.IsErrorRedirected;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public bool IsInputRedirected => this._terminal.IsInputRedirected;

    private CommandLineConsole(ITerminal terminal)
    {
        this._terminal = terminal;

        this.Out = new TerminalStreamWriter(terminal.Out);
        this.Error = new TerminalStreamWriter(terminal.Error);
    }

    /// <summary>
    /// Returns the <see cref="IConsole"/> instance for <paramref name="terminal"/>.
    /// </summary>
    public static IConsole? FromTerminal(ITerminal terminal)
    {
        if (ReferenceEquals(terminal, Terminal.Instance))
        {
            // IMPORTANT: We must return "null" here so that we can get properly aligned
            //   help texts. Unfortunately, alignment is only supported if "IConsole"
            //   is "null".
            //
            //   Note, however, that despite https://github.com/dotnet/command-line-api/issues/1174#issuecomment-770774549
            //   and https://github.com/dotnet/command-line-api/issues/1184#issuecomment-822001709
            //   returning "null" here is still the best solution (and it doesn't need to be fixed
            //   with using "HelpBuilder" somehow).
            return null;
        }
        else
        {
            return new CommandLineConsole(terminal);
        }
    }

    private sealed class TerminalStreamWriter : IStandardStreamWriter
    {
        private readonly ITerminalWriter _terminalWriter;

        public TerminalStreamWriter(ITerminalWriter terminalWriter)
        {
            this._terminalWriter = terminalWriter;
        }

        /// <inheritdoc />
        public void Write(string? value)
        {
            this._terminalWriter.Write(value);
        }
    }
}
