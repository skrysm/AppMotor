// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using JetBrains.Annotations;

namespace AppMotor.CoreKit.Extensions;

/// <summary>
/// Extension methods for <see cref="DirectoryInfo"/>.
/// </summary>
public static class DirectoryInfoExtensions
{
    /// <summary>
    /// Returns a <see cref="FileInfo"/> object for the specified <paramref name="fileName"/>
    /// within this folder.
    /// </summary>
    /// <remarks>
    /// This method uses <see cref="Path.Combine(string, string)"/> - so the resulting <see cref="FileInfo"/>
    /// may not actually be inside this folder but just relative to it.
    /// </remarks>
    [PublicAPI, MustUseReturnValue]
    public static FileInfo GetFile(this DirectoryInfo dir, string fileName)
    {
        return new FileInfo(Path.Combine(dir.FullName, fileName));
    }
}
