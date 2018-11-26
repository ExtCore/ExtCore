// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.FileStorage.Abstractions;

namespace ExtCore.FileStorage.FileSystem
{
  /// <summary>
  /// Implements the <see cref="IDirectoryProxy">IDirectoryProxy</see> interface and represents a directory in a file system.
  /// </summary>
  public class DirectoryProxy : IDirectoryProxy
  {
    private readonly string rootPath;
    private readonly string path;

    /// <summary>
    /// The path of the underlying directory relatively to the root one.
    /// </summary>
    public string RelativePath { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryProxy">DirectoryProxy</see> class.
    /// </summary>
    /// <param name="rootPath">The root path of the underlying directory's relative one.</param>
    /// <param name="relativePath">The path of the underlying directory relatively to the root one.</param>
    public DirectoryProxy(string rootPath, string relativePath)
    {
      this.rootPath = AbsolutePath.Combine(rootPath);
      this.RelativePath = AbsolutePath.Combine(relativePath);
      this.path = AbsolutePath.Combine(this.rootPath, this.RelativePath);
    }

    /// <summary>
    /// Checks if the underlying directory exists.
    /// </summary>
    /// <returns>Returns a flag indicating if the underlying directory exists.</returns>
    public async Task<bool> ExistsAsync()
    {
      return await Task<bool>.Factory.StartNew(() => Directory.Exists(this.path));
    }

    /// <summary>
    /// Creates the underlying directory.
    /// </summary>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task CreateAsync()
    {
      await Task.Factory.StartNew(() =>
      {
        try
        {
          Directory.CreateDirectory(this.path);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.path}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.path}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.path}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
        }
      });
    }

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
    public async Task MoveAsync(string destinationRelativePath)
    {
      if (destinationRelativePath == string.Empty)
        throw new ArgumentException($"Value can't be empty. Parameter name: destinationRelativePath.");

      if (destinationRelativePath == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: destinationRelativePath.", default(Exception));

      await Task.Factory.StartNew(() =>
      {
        try
        {
          Directory.Move(this.path, this.rootPath + destinationRelativePath);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.path}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.path}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.path}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
        }
      });
    }

    /// <summary>
    /// Deletes the underlying directory.
    /// </summary>
    /// <param name="recursive">Pass true to remove all the underlying directory content recursively; otherwise false.</param>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task DeleteAsync(bool recursive)
    {
      await Task.Factory.StartNew(() =>
      {
        try
        {
          Directory.Delete(this.path, recursive);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.path}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.path}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.path}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
        }
      });
    }

    /// <summary>
    /// Gets the directory proxies for the directories inside the underlying one.
    /// </summary>
    /// <returns>The directory proxies for the directories inside the underlying one</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task<IEnumerable<IDirectoryProxy>> GetDirectoryProxiesAsync()
    {
      return await Task<IEnumerable<IDirectoryProxy>>.Factory.StartNew(
        () =>
        {
          try
          {
            return Directory.GetDirectories(this.path).Select(
              d => new DirectoryProxy(this.rootPath, d.Substring(this.rootPath.Length))
            );
          }

          catch (UnauthorizedAccessException e)
          {
            throw new AccessDeniedException($"Access denied: \"{this.path}\". See inner exception for details.", e);
          }

          catch (System.IO.DirectoryNotFoundException e)
          {
            throw new DirectoryNotFoundException($"Directory not found: \"{this.path}\". See inner exception for details.", e);
          }

          catch (System.IO.PathTooLongException e)
          {
            throw new PathTooLongException($"Path too long: \"{this.path}\". See inner exception for details.", e);
          }

          catch (Exception e)
          {
            throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
          }
        }
      );
    }

    /// <summary>
    /// Gets the file proxies for the files inside the underlying one.
    /// </summary>
    /// <returns>The file proxies for the files inside the underlying directory.</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task<IEnumerable<IFileProxy>> GetFileProxiesAsync()
    {
      return await Task<IEnumerable<IFileProxy>>.Factory.StartNew(
        () =>
        {
          try
          {
            return Directory.GetFiles(this.path).Select(
              f =>
              {
                string relativePath = f;

                relativePath = relativePath.Substring(this.rootPath.Length);
                relativePath = relativePath.Remove(relativePath.LastIndexOf(Path.DirectorySeparatorChar));
                return new FileProxy(this.rootPath, relativePath, f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar) + 1));
              }
            );
          }

          catch (UnauthorizedAccessException e)
          {
            throw new AccessDeniedException($"Access denied: \"{this.path}\". See inner exception for details.", e);
          }

          catch (System.IO.DirectoryNotFoundException e)
          {
            throw new DirectoryNotFoundException($"Directory not found: \"{this.path}\". See inner exception for details.", e);
          }

          catch (System.IO.PathTooLongException e)
          {
            throw new PathTooLongException($"Path too long: \"{this.path}\". See inner exception for details.", e);
          }

          catch (Exception e)
          {
            throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
          }
        }
      );
    }
  }
}