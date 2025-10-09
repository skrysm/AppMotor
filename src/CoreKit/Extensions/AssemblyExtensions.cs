// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Reflection;

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
    /// Returns an "accessor" to the embedded resources of this assembly.
    /// </summary>
    [MustUseReturnValue]
    public static AssemblyEmbeddedResources GetEmbeddedResources(this Assembly assembly)
    {
        return new AssemblyEmbeddedResources(assembly);
    }
}
