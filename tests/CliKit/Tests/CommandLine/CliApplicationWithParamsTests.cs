// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CliKit.CommandLine;
using AppMotor.CliKit.Terminals.Formatting;
using AppMotor.CliKit.TestUtils;
using AppMotor.CoreKit.Exceptions;

using Shouldly;

using Xunit;

namespace AppMotor.CliKit.Tests.CommandLine;

public sealed class CliApplicationWithParamsTests
{
    [Fact]
    public void TestExceptionHandling_Regular()
    {
        var app = new ExceptionTestApplication(throwErrorMessageException: false);

        var caughtException = app.RunWithExpectedException();

        caughtException.ShouldNotBeNull();
        caughtException.ShouldBeOfType<InvalidOperationException>();
        caughtException.Message.ShouldBe("This is a test");

        app.TerminalOutput.ShouldContain("This is a test");
        app.TerminalOutput.ShouldContain($"[{typeof(InvalidOperationException).FullName}] "); // <-- Indicates the full exception report has been printed
    }

    [Fact]
    public void TestExceptionHandling_ErrorMessageException()
    {
        var app = new ExceptionTestApplication(throwErrorMessageException: true);

        var caughtException = app.RunWithExpectedException(expectedExitCode: 1);

        caughtException.ShouldNotBeNull();
        caughtException.ShouldBeOfType<ErrorMessageException>();
        caughtException.Message.ShouldBe("This is an error message.");

        // This must be the only output.
        app.TerminalOutput.Trim().ShouldBe(TermText.Red("This is an error message."));
    }

    private sealed class ExceptionTestApplication : TestApplicationWithParamsBase
    {
        /// <inheritdoc />
        protected override CliCommandExecutor Executor => new(Execute);

        /// <inheritdoc />
        protected override int ExitCodeOnException => 42;

        private readonly bool _throwErrorMessageException;

        /// <inheritdoc />
        public ExceptionTestApplication(bool throwErrorMessageException)
        {
            this._throwErrorMessageException = throwErrorMessageException;
        }

        private void Execute()
        {
            if (this._throwErrorMessageException)
            {
                throw new ErrorMessageException("This is an error message.");
            }
            else
            {
                throw new InvalidOperationException("This is a test");
            }
        }
    }
}
