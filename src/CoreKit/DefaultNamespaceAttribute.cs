// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Extensions;
using AppMotor.CoreKit.Utils;

using JetBrains.Annotations;

namespace AppMotor.CoreKit;

/// <summary>
/// Stores the assembly's default namespace. The default namespace is used by default for EmbeddedResource names.
/// </summary>
/// <remarks>
/// This attribute is automatically created for any assembly that directly or indirectly references
/// the CoreKit during build (unless the <c>&lt;CreateDefaultNamespaceAttribute&gt;</c> property is
/// set to <c>false</c>).
/// </remarks>
/// <remarks>
/// In MSBuild, this value is called "root namespace" instead of "default namespace".
/// </remarks>
/// <seealso cref="AssemblyExtensions.GetDefaultNamespace"/>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class DefaultNamespaceAttribute : Attribute
{
    /// <summary>
    /// The name of the default namespace (often the same as the assembly name).
    /// </summary>
    public string NamespaceName { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    [UsedImplicitly]
    public DefaultNamespaceAttribute(string namespaceName)
    {
        Validate.ArgumentWithName(nameof(namespaceName)).IsNotNullOrWhiteSpace(namespaceName);

        this.NamespaceName = namespaceName;
    }
}
