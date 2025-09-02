﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.IO.Abstractions;

using JetBrains.Annotations;

namespace AppMotor.CoreKit.IO;

/// <summary>
/// Provides a convenience way to specify the actual file system when working with <c>System.IO.Abstractions</c>.
/// </summary>
public static class RealFileSystem
{
    /// <summary>
    /// The real file system.
    /// </summary>
    [PublicAPI]
    public static IFileSystem Instance { get; } = new FileSystem();
}
