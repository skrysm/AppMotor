using JetBrains.Annotations;

namespace AppMotor.HttpServerKit.Samples.WebApi.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class TodoItem
{
    public long Id { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}
