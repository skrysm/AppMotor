// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

namespace AppMotor.CoreKit.Certificates.Pem;

/// <summary>
/// Exception thrown for format errors in PEM files.
/// </summary>
public sealed class PemFormatException : Exception
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public PemFormatException(string? message) : base(message)
    {
    }
}
