// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;

namespace ExtCore.FileStorage.Abstractions
{
  /// <summary>
  /// Describes a generic file proxy to manipulate an underlying file with a specified relative path and a filename.
  /// </summary>
  public interface IFileProxy
  {
    /// <summary>
    /// The path of the underlying file relatively to the root one.
    /// </summary>
    string RelativePath { get; }

    /// <summary>
    /// The filename of the underlying file.
    /// </summary>
    string Filename { get; }

    /// <summary>
    /// Checks if the underlying file exists.
    /// </summary>
    /// <returns>Returns a flag indicating if the underlying file exists.</returns>
    Task<bool> ExistsAsync();

    /// <summary>
    /// Reads content of the underlying file as a stream.
    /// </summary>
    /// <returns>Content of the underlying file as a stream.</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task<Stream> ReadStreamAsync();

    /// <summary>
    /// Reads content of the underlying file as a byte array.
    /// </summary>
    /// <returns>Content of the underlying file as a byte array.</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task<byte[]> ReadBytesAsync();

    /// <summary>
    /// Reads content of the underlying file as a text string.
    /// </summary>
    /// <returns>Content of the underlying file as a text string.</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task<string> ReadTextAsync();

    /// <summary>
    /// Writes content to the underlying file as a stream.
    /// </summary>
    /// <param name="inputStream">Content to write to the underlying file as a stream.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task WriteStreamAsync(Stream inputStream);

    /// <summary>
    /// Writes content to the underlying file as a byte array.
    /// </summary>
    /// <param name="bytes">Content to write to the underlying file as a byte array.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task WriteBytesAsync(byte[] bytes);

    /// <summary>
    /// Writes content to the underlying file as a text string.
    /// </summary>
    /// <param name="text">Content to write to the underlying file as a text string.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task WriteTextAsync(string text);

    /// <summary>
    /// Deletes the underlying file.
    /// </summary>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task DeleteAsync();
  }
}