﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AppMotor.CliApp.Terminals.Formatting;

/// <summary>
/// Takes care about ANSI escape sequence support on Windows - see <see cref="Enable"/>.
/// </summary>
public static partial class AnsiSupportOnWindows
{
    private const int STD_OUTPUT_HANDLE = -11;

    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    private static readonly IntPtr INVALID_HANDLE_VALUE = new(-1);

    /// <summary>
    /// Enables ANSI escape sequence support on Windows. Has no effect on non-Windows
    /// operating systems.
    /// </summary>
    /// <returns>Returns whether ANSI escape sequence support could be enabled.
    /// On non-Windows operating systems, this always returns <c>true</c>.</returns>
    /// <remarks>
    /// ANSI escape sequences are support on Windows 10 and higher and on Windows Server 2019
    /// and higher. For Windows Server 2016, this method will return <c>true</c> but Windows
    /// will "downgrade" all colors to the default 16 colors.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public static bool Enable()
    {
        if (!OperatingSystem.IsWindows())
        {
            return true;
        }

        var stdOutHandle = GetStdHandle(STD_OUTPUT_HANDLE);
        if (stdOutHandle == INVALID_HANDLE_VALUE)
        {
            return false;
        }

        if (!GetConsoleMode(stdOutHandle, out uint consoleMode))
        {
            return false;
        }

        if ((consoleMode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) != 0)
        {
            // Already enabled (for example on Windows 11).
            return true;
        }

        return SetConsoleMode(stdOutHandle, consoleMode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
    }

    [LibraryImport("kernel32.dll")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [LibraryImport("kernel32.dll")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static partial IntPtr GetStdHandle(int nStdHandle);
}
