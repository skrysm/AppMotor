// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

namespace AppMotor.CoreKit.Exceptions;

/// <summary>
/// <see cref="ArgumentException"/> version of <see cref="CollectionIsReadOnlyException"/>.
/// </summary>
public class CollectionIsReadOnlyArgumentException : ArgumentException, ICollectionIsReadOnlyException
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="paramName">The name of the parameter this exception applies to.</param>
    public CollectionIsReadOnlyArgumentException(string paramName)
        : base(CollectionIsReadOnlyException.DEFAULT_MESSAGE, paramName: paramName)
    {
    }
}
