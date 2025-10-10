// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Reflection;
using System.Resources;

using AppMotor.CoreKit.Exceptions;
using AppMotor.CoreKit.Utils;

using JetBrains.Annotations;

namespace AppMotor.CoreKit.Extensions;

/// <summary>
/// Extension methods for <see cref="Assembly"/>.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Returns the simple name for this assembly. This is usually, but not necessarily, the file
    /// name of the manifest file of the assembly, minus its extension.
    /// </summary>
    [MustUseReturnValue]
    public static string GetSimpleName(this Assembly assembly)
    {
        return assembly.GetName().Name ?? throw new UnexpectedBehaviorException($"Simple name is not available for assembly '{assembly}'.");
    }

    /// <summary>
    /// Just a "helper" method to make it easier to locate the actual path property: <see cref="Assembly.Location"/>
    /// </summary>
    [Obsolete("Use Assembly.Location instead.")]
    [PublicAPI, MustUseReturnValue]
    public static string GetPath(this Assembly assembly)
    {
        return assembly.Location;
    }

    /// <summary>
    /// Returns the default namespace for the specified assembly.
    /// </summary>
    /// <remarks>
    /// For this method to work, the default namespace must be stored in the <see cref="DefaultNamespaceAttribute"/>.
    /// This attribute is automatically created for any assembly that directly or indirectly references
    /// the CoreKit during build (unless the <c>&lt;CreateDefaultNamespaceAttribute&gt;</c> property is
    /// set to <c>false</c>).
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown if the default namespace hasn't been stored in the
    /// <see cref="DefaultNamespaceAttribute"/>.</exception>
    [PublicAPI, MustUseReturnValue]
    public static string GetDefaultNamespace(this Assembly assembly)
    {
        var attribute = assembly.GetCustomAttribute<DefaultNamespaceAttribute>();

        if (attribute is null)
        {
            throw new InvalidOperationException("The default namespace hasn't been stored in this assembly.");
        }

        return attribute.NamespaceName;
    }

    /// <summary>
    /// Returns whether the specified embedded resource exists in this assembly.
    /// </summary>
    /// <param name="assembly">this assembly</param>
    /// <param name="resourceName">The full resource name (case-sensitive).</param>
    [MustUseReturnValue]
    public static bool DoesEmbeddedResourceExist(this Assembly assembly, string resourceName)
    {
        return assembly.GetManifestResourceInfo(resourceName) is not null;
    }

    /// <summary>
    /// Returns an "accessor" to the specified embedded resource of this assembly. Throws a <see cref="MissingManifestResourceException"/>
    /// if the resource doesn't exist.
    /// </summary>
    /// <param name="assembly">this assembly</param>
    /// <param name="resourceName">The full resource name (case-sensitive).</param>
    [MustUseReturnValue]
    public static EmbeddedResource GetEmbeddedResource(this Assembly assembly, string resourceName)
    {
        return assembly.GetEmbeddedResourceOrNull(resourceName)
            ?? throw new MissingManifestResourceException($"The embedded resource '{resourceName}' was not found in assembly '{assembly.GetSimpleName()}'.");
    }

    /// <summary>
    /// Returns an "accessor" to the specified embedded resource of this assembly. Throws a <see cref="MissingManifestResourceException"/>
    /// if the resource doesn't exist.
    /// </summary>
    /// <param name="assembly">this assembly</param>
    /// <param name="type">The type whose namespace is used to scope the <paramref name="resourceName"/>.</param>
    /// <param name="resourceName">The resource name (case-sensitive).</param>
    [MustUseReturnValue]
    public static EmbeddedResource GetEmbeddedResource(this Assembly assembly, Type type, string resourceName)
    {
        return assembly.GetEmbeddedResource(EmbeddedResource.ConstructResourceName(type, resourceName));
    }

    /// <summary>
    /// Returns an "accessor" to the specified embedded resource of this assembly. Returns <c>null</c> if
    /// the resource doesn't exist.
    /// </summary>
    /// <param name="assembly">this assembly</param>
    /// <param name="resourceName">The full resource name (case-sensitive).</param>
    [MustUseReturnValue]
    public static EmbeddedResource? GetEmbeddedResourceOrNull(this Assembly assembly, string resourceName)
    {
        if (!assembly.DoesEmbeddedResourceExist(resourceName))
        {
            return null;
        }

        return new EmbeddedResource(assembly, resourceName);
    }

    /// <summary>
    /// Returns an "accessor" to the specified embedded resource of this assembly. Returns <c>null</c> if
    /// the resource doesn't exist.
    /// </summary>
    /// <param name="assembly">this assembly</param>
    /// <param name="type">The type whose namespace is used to scope the <paramref name="resourceName"/>.</param>
    /// <param name="resourceName">The resource name (case-sensitive).</param>
    [MustUseReturnValue]
    public static EmbeddedResource? GetEmbeddedResourceOrNull(this Assembly assembly, Type type, string resourceName)
    {
        return assembly.GetEmbeddedResourceOrNull(EmbeddedResource.ConstructResourceName(type, resourceName));
    }
}
