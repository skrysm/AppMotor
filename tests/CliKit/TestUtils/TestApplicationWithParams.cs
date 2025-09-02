// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CliKit.CommandLine;

namespace AppMotor.CliKit.TestUtils;

internal class TestApplicationWithParams : TestApplicationWithParamsBase
{
    private readonly Action _mainAction;

    private readonly List<CliParamBase> _params = [];

    /// <inheritdoc />
    protected override CliCommandExecutor Executor => new(Execute);

    public TestApplicationWithParams(Action mainAction, params CliParamBase[] cliParams)
    {
        this._mainAction = mainAction;

        this._params.AddRange(cliParams);
    }

    /// <inheritdoc />
    protected override IEnumerable<CliParamBase> GetAllParams()
    {
        return this._params;
    }

    private void Execute()
    {
        this._mainAction();
    }
}
