// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using AppMotor.CoreKit.Exceptions;

using JetBrains.Annotations;

namespace AppMotor.CoreKit.Extensions;

/// <summary>
/// Extension methods for <see cref="Stream"/>.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Reads the whole stream into a byte array.
    /// </summary>
    /// <remarks>
    /// Make sure not to read unbound, untrusted data - or this operation may result in a
    /// <see cref="OutOfMemoryException"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown if this stream can't be read.</exception>
    [MustUseReturnValue]
    public static byte[] ReadAsBytes(this Stream stream)
    {
        if (!stream.CanRead)
        {
            throw new InvalidOperationException("This stream can't be read.");
        }

        var streamLength = stream.GetRemainingLength();
        if (streamLength == 0)
        {
            return [];
        }

        if (streamLength is null)
        {
            // Length is unknown
            using var memStream = new MemoryStream();

            stream.CopyTo(memStream);

            return memStream.ToArray();
        }
        else
        {
            // Length is known.
            var buffer = new byte[streamLength.Value];

            stream.ReadExactly(buffer.AsSpan());

            if (stream.ReadByte() != -1)
            {
                throw new InvalidOperationException($"The stream has reported a remaining length of {streamLength} bytes there are still bytes in the stream.");
            }

            return buffer;
        }
    }

    /// <summary>
    /// Reads the whole stream into a byte array.
    /// </summary>
    /// <remarks>
    /// Make sure not to read unbound, untrusted data - or this operation may result in a
    /// <see cref="OutOfMemoryException"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown if this stream can't be read.</exception>
    [MustUseReturnValue]
    public static async Task<byte[]> ReadAsBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        if (!stream.CanRead)
        {
            throw new InvalidOperationException("This stream can't be read.");
        }

        var streamLength = stream.GetRemainingLength();
        if (streamLength == 0)
        {
            return [];
        }

        if (streamLength is null)
        {
            // Length is unknown
            using var memStream = new MemoryStream();

            await stream.CopyToAsync(memStream, cancellationToken).ConfigureAwait(false);

            return memStream.ToArray();
        }
        else
        {
            // Length is known.
            var buffer = new byte[streamLength.Value];

            await stream.ReadExactlyAsync(buffer.AsMemory(), cancellationToken).ConfigureAwait(false);

            if (stream.ReadByte() != -1)
            {
                throw new InvalidOperationException($"The stream has reported a remaining length of {streamLength} bytes there are still bytes in the stream.");
            }

            return buffer;
        }
    }

    [MustUseReturnValue]
    private static int? GetRemainingLength(this Stream stream)
    {
        if (!stream.CanSeek)
        {
            // Seeking is required to determine the stream's position.
            return null;
        }

        long streamLength;
        try
        {
            streamLength = stream.Length;
        }
        catch (NotSupportedException)
        {
            return null;
        }

        if (streamLength <= 0)
        {
            return null;
        }

        long streamPosition;
        try
        {
            streamPosition = stream.Position;
        }
        catch (NotSupportedException)
        {
            return null;
        }

        if (streamPosition < 0)
        {
            return null;
        }

        var remainingLength = streamLength - streamPosition;
        if (remainingLength > int.MaxValue)
        {
            throw new InvalidOperationException("The stream won't fit into a byte array.");
        }
        if (remainingLength < 0)
        {
            throw new UnexpectedBehaviorException($"The remaining length of the stream is negative ({remainingLength}).");
        }

        return (int)remainingLength;
    }
}
