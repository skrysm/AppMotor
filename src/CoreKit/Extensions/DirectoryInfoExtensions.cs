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
    /// within this folder. Note that the file doesn't need to exist.
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

    /// <summary>
    /// Returns a <see cref="DirectoryInfo"/> object for the specified <paramref name="dirName"/>
    /// within this folder. Note that the directory doesn't need to exist.
    /// </summary>
    /// <remarks>
    /// This method uses <see cref="Path.Combine(string, string)"/> - so the resulting <see cref="DirectoryInfo"/>
    /// may not actually be inside this folder but just relative to it.
    /// </remarks>
    [MustUseReturnValue]
    public static DirectoryInfo GetDirectory(this DirectoryInfo dir, string dirName)
    {
        return new DirectoryInfo(Path.Combine(dir.FullName, dirName));
    }
}
