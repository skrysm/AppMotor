// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using JetBrains.Annotations;

namespace AppMotor.CoreKit.Exceptions;

/// <summary>
/// Thrown if you try to access properties of an uninitialized struct.
/// </summary>
[PublicAPI]
public class UninitializedStructPropertyException : InvalidOperationException;
