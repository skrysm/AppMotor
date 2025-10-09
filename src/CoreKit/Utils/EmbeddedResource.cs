// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

using AppMotor.CoreKit.Exceptions;
using AppMotor.CoreKit.Extensions;

using JetBrains.Annotations;

namespace AppMotor.CoreKit.Utils;

/// <summary>
/// Provides an "accessor" to an embedded resource of an assembly.
/// </summary>
[SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "No really a data class")]
public readonly struct EmbeddedResource
{
    private readonly Assembly _assembly;

    private readonly string _resourceName;

    /// <summary>
    /// Constructor for existing resource.
    /// </summary>
    internal EmbeddedResource(Assembly assembly, string resourceName)
    {
        Validate.ArgumentWithName(nameof(assembly)).IsNotNull(assembly);
        Validate.ArgumentWithName(nameof(resourceName)).IsNotNullOrWhiteSpace(resourceName);

        this._assembly = assembly;
        this._resourceName = resourceName;
    }

    /// <summary>
    /// Constructs the resource name from the type's namespace and the resource name.
    /// </summary>
    /// <param name="type">The type whose namespace is used to scope the <paramref name="resourceName"/>.</param>
    /// <param name="resourceName">The resource name (case-sensitive).</param>
    [MustUseReturnValue]
    public static string ConstructResourceName(Type type, string resourceName)
    {
        var nameSpace = type.Namespace;

        char c = Type.Delimiter;

        return string.Concat(nameSpace, new ReadOnlySpan<char>(in c), resourceName);
    }

    /// <summary>
    /// Returns a <see cref="Stream"/> for this resource.
    /// </summary>
    [MustUseReturnValue, MustDisposeResource]
    public Stream AsStream()
    {
        // NOTE: We throw an UnexpectedBehaviorException() here because the premise of this type is that the resource exists.
        return this._assembly.GetManifestResourceStream(this._resourceName)
            ?? throw new UnexpectedBehaviorException($"The embedded resource '{this._resourceName}' doesn't exist in assembly '{this._assembly.GetSimpleName()}'.");
    }

    /// <summary>
    /// Returns this resource as byte array.
    /// </summary>
    /// <returns></returns>
    [MustUseReturnValue]
    public byte[] AsBytes()
    {
        using var stream = AsStream();
        return stream.ReadAsBytes();
    }

    /// <summary>
    /// Returns this resource as string.
    /// </summary>
    [MustUseReturnValue]
    public string AsString(Encoding encoding)
    {
        using var stream = AsStream();
        return stream.ReadAsString(encoding);
    }

    /// <summary>
    /// Returns this resource as lines.
    /// </summary>
    [MustUseReturnValue]
    public IEnumerable<string> AsLines(Encoding encoding)
    {
        // NOTE: This is generally safe as this line will only be called when "IEnumerable<string>.GetEnumerator()" is called -
        //   NOT when this method is called. (It's the C# iterator magic.)
        using var stream = AsStream();

        // IMPORTANT: We must NOT return the result of "ReadAsLine()" directly here because
        //   we need to make sure the stream is only disposed after all (required) lines have
        //   been read.
        foreach (var line in stream.ReadAsLines(encoding))
        {
            yield return line;
        }
    }
}
