﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Exceptions;
using AppMotor.CoreKit.Utils;

using JetBrains.Annotations;

namespace AppMotor.CoreKit.Extensions;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/> and <see cref="ICollection{T}"/>.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Returns this enumeration without any <c>null</c> elements.
    /// </summary>
    [MustUseReturnValue]
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> collection)
    {
        return collection.Where(item => item is not null)!;
    }

    /// <summary>
    /// Returns this enumeration without any <c>null</c> elements.
    /// </summary>
    [MustUseReturnValue]
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> collection) where T : struct
    {
        return collection.Where(item => item is not null).Select(item => item!.Value);
    }

    /// <summary>
    /// Adds the specified items to the collection.
    /// </summary>
    /// <exception cref="CollectionIsReadOnlyException">Thrown if the collection is read-only.</exception>
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> itemsToAdd)
    {
        Validate.ArgumentWithName(nameof(collection)).IsNotNull(collection);
        Validate.ArgumentWithName(nameof(collection)).IsNotReadOnly(collection);
        Validate.ArgumentWithName(nameof(itemsToAdd)).IsNotNull(itemsToAdd);

        foreach (var item in itemsToAdd)
        {
            collection.Add(item);
        }
    }

    /// <summary>
    /// Removes the items from the collection where the <paramref name="predicate"/> returns true.
    /// </summary>
    /// <returns>Returns the items that were removed from the collection. If no items were
    /// removed, the returned list will be empty (but never <c>null</c>).</returns>
    /// <exception cref="CollectionIsReadOnlyException">Thrown if the collection is read-only.</exception>
#pragma warning disable CA1002 // Do not expose generic lists // BUG: https://github.com/dotnet/roslyn-analyzers/issues/4508
    public static List<T> RemoveWhere<T>(this ICollection<T> collection, Predicate<T> predicate)
#pragma warning restore CA1002 // Do not expose generic lists
    {
        Validate.ArgumentWithName(nameof(collection)).IsNotNull(collection);
        Validate.ArgumentWithName(nameof(collection)).IsNotReadOnly(collection);
        Validate.ArgumentWithName(nameof(predicate)).IsNotNull(predicate);

        var leftOver = new List<T>(collection.Count);
        var removed = new List<T>(collection.Count);

        foreach (var item in collection)
        {
            if (predicate(item))
            {
                removed.Add(item);
            }
            else
            {
                leftOver.Add(item);
            }
        }

        collection.Clear();
        collection.AddRange(leftOver);

        return removed;
    }

}
