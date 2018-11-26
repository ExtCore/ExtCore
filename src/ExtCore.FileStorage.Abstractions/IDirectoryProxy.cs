// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExtCore.FileStorage.Abstractions
{
  /// <summary>
  /// Describes a generic directory proxy to manipulate an underlying directory with a specified relative path.
  /// </summary>
  public interface IDirectoryProxy
  {
    /// <summary>
    /// The path of the underlying directory relatively to the root one.
    /// </summary>
    string RelativePath { get; }

    /// <summary>
    /// Checks if the underlying directory exists.
    /// </summary>
    /// <returns>Returns a flag indicating if the underlying directory exists.</returns>
    Task<bool> ExistsAsync();

    /// <summary>
    /// Creates the underlying directory.
    /// </summary>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task CreateAsync();

    /// <summary>
    /// Moves the underlying directory.
    /// </summary>
    /// <param name="destinationRelativePath"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task MoveAsync(string destinationRelativePath);

    /// <summary>
    /// Deletes the underlying directory.
    /// </summary>
    /// <param name="recursive">Pass true to remove all the underlying directory content recursively; otherwise false.</param>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task DeleteAsync(bool recursive);

    /// <summary>
    /// Gets the directory proxies for the directories inside the underlying one.
    /// </summary>
    /// <returns>The directory proxies for the directories inside the underlying one</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task<IEnumerable<IDirectoryProxy>> GetDirectoryProxiesAsync();

    /// <summary>
    /// Gets the file proxies for the files inside the underlying one.
    /// </summary>
    /// <returns>The file proxies for the files inside the underlying directory.</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    Task<IEnumerable<IFileProxy>> GetFileProxiesAsync();
  }
}