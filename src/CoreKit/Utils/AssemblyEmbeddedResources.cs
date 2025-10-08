// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;

using JetBrains.Annotations;

namespace AppMotor.CoreKit.Utils;

/// <summary>
/// Provides an "accessor" to the embedded resources of an assembly.
/// </summary>
[SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "No really a data class")]
public readonly struct AssemblyEmbeddedResources
{
    private readonly Assembly _assembly;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <remarks>
    /// Instead of using this constructor, you may also call <see cref="Extensions.AssemblyExtensions.GetEmbeddedResources"/>.
    /// </remarks>
    public AssemblyEmbeddedResources(Assembly assembly)
    {
        Validate.ArgumentWithName(nameof(assembly)).IsNotNull(assembly);

        this._assembly = assembly;
    }

    /// <summary>
    /// Returns the stream to the specified embedded resource. Returns <c>null</c> if
    /// the resource doesn't exist.
    /// </summary>
    /// <param name="resourceName">The full resource name (case-sensitive).</param>
    /// <seealso cref="GetRequiredResource(string)"/>
    [MustUseReturnValue, MustDisposeResource]
    public Stream? GetResource(string resourceName)
    {
        return this._assembly.GetManifestResourceStream(resourceName);
    }

    /// <summary>
    /// Returns the stream to the specified embedded resource. Returns <c>null</c> if
    /// the resource doesn't exist.
    /// </summary>
    /// <param name="type">The type whose namespace is used to scope the <paramref name="resourceName"/>.</param>
    /// <param name="resourceName">The resource name (case-sensitive).</param>
    /// <seealso cref="GetRequiredResource(Type, string)"/>
    [MustUseReturnValue, MustDisposeResource]
    public Stream? GetResource(Type type, string resourceName)
    {
        return this._assembly.GetManifestResourceStream(type, resourceName);
    }

    /// <summary>
    /// Returns the stream to the specified embedded resource. Throws a <see cref="MissingManifestResourceException"/>
    /// if the resource doesn't exist.
    /// </summary>
    /// <param name="resourceName">The full resource name (case-sensitive).</param>
    /// <seealso cref="GetResource(string)"/>
    [MustUseReturnValue, MustDisposeResource]
    public Stream GetRequiredResource(string resourceName)
    {
        return GetResource(resourceName)
            ?? throw new MissingManifestResourceException($"The embedded resource '{resourceName}' was not found in assembly '{this._assembly.GetName().Name}'.");
    }

    /// <summary>
    /// Returns the stream to the specified embedded resource. Throws a <see cref="MissingManifestResourceException"/>
    /// if the resource doesn't exist.
    /// </summary>
    /// <param name="type">The type whose namespace is used to scope the <paramref name="resourceName"/>.</param>
    /// <param name="resourceName">The resource name (case-sensitive).</param>
    /// <seealso cref="GetResource(Type, string)"/>
    [MustUseReturnValue, MustDisposeResource]
    public Stream GetRequiredResource(Type type, string resourceName)
    {
        Validate.ArgumentWithName(nameof(type)).IsNotNull(type);

        return GetResource(type, resourceName)
            ?? throw new MissingManifestResourceException($"The embedded resource '{type.Namespace}.{resourceName}' was not found in assembly '{this._assembly.GetName().Name}'.");
    }
}
