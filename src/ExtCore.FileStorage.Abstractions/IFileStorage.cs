// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ExtCore.FileStorage.Abstractions
{
  /// <summary>
  /// Describes a generic file storage that allows to manipulate directories and files via proxies.
  /// </summary>
  public interface IFileStorage
  {
    /// <summary>
    /// Creates a directory proxy which allows to manipulate an underlying directory with a specified relative path.
    /// </summary>
    /// <param name="relativePath">The path of the underlying directory relatively to the root one.</param>
    /// <returns>Created directory proxy.</returns>
    IDirectoryProxy CreateDirectoryProxy(string relativePath);

    /// <summary>
    /// Creates a file proxy which allows to manipulate an underlying file with a specified relative path and a filename.
    /// </summary>
    /// <param name="relativePath">The path of the underlying file relatively to the root one.</param>
    /// <param name="filename">The filename of the underlying file.</param>
    /// <returns>Created file proxy.</returns>
    IFileProxy CreateFileProxy(string relativePath, string filename);
  }
}