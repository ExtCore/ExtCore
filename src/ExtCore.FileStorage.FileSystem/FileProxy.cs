// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using ExtCore.FileStorage.Abstractions;

namespace ExtCore.FileStorage.FileSystem
{
  /// <summary>
  /// Implements the <see cref="IDirectoryProxy">IDirectoryProxy</see> interface and represents a file in a file system.
  /// </summary>
  public class FileProxy : IFileProxy
  {
    private readonly string rootPath;
    private readonly string filepath;

    /// <summary>
    /// The path of the underlying file relatively to the root one.
    /// </summary>
    public string RelativePath { get; private set; }

    /// <summary>
    /// The filename of the underlying file.
    /// </summary>
    public string Filename { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileProxy">FileProxy</see> class.
    /// </summary>
    /// <param name="rootPath">The root path of the underlying file's relative one.</param>
    /// <param name="relativePath">The path of the underlying file relatively to the root one.</param>
    /// <param name="filename">The filename of the underlying file.</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public FileProxy(string rootPath, string relativePath, string filename)
    {
      if (filename == string.Empty)
        throw new ArgumentException($"Value can't be empty. Parameter name: filename.");

      if (filename == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: filename.", default(Exception));

      this.rootPath = rootPath;
      this.RelativePath = relativePath;
      this.Filename = filename;
      this.filepath = AbsolutePath.Combine(this.rootPath, this.RelativePath, this.Filename);
    }

    /// <summary>
    /// Checks if the underlying file exists.
    /// </summary>
    /// <returns>Returns a flag indicating if the underlying file exists.</returns>
    public async Task<bool> ExistsAsync()
    {
      return await Task<bool>.Factory.StartNew(() => File.Exists(this.filepath));
    }

    /// <summary>
    /// Reads content of the underlying file as a stream.
    /// </summary>
    /// <returns>Content of the underlying file as a stream.</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task<Stream> ReadStreamAsync()
    {
      return await Task<Stream>.Factory.StartNew(() =>
      {
        try
        {
          return File.Open(this.filepath, FileMode.Open);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.FileNotFoundException e)
        {
          throw new FileNotFoundException($"File not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
        }
      });
    }

    /// <summary>
    /// Reads content of the underlying file as a byte array.
    /// </summary>
    /// <returns>Content of the underlying file as a byte array.</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task<byte[]> ReadBytesAsync()
    {
      return await Task<byte[]>.Factory.StartNew(() =>
      {
        try
        {
          return File.ReadAllBytes(this.filepath);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (SecurityException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.FileNotFoundException e)
        {
          throw new FileNotFoundException($"File not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
        }
      });
    }

    /// <summary>
    /// Reads content of the underlying file as a text string.
    /// </summary>
    /// <returns>Content of the underlying file as a text string.</returns>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task<string> ReadTextAsync()
    {
      return await Task<string>.Factory.StartNew(() =>
      {
        try
        {
          return File.ReadAllText(this.filepath);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (SecurityException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.FileNotFoundException e)
        {
          throw new FileNotFoundException($"File not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
        }
      });
    }

    /// <summary>
    /// Writes content to the underlying file as a stream.
    /// </summary>
    /// <param name="inputStream">Content to write to the underlying file as a stream.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task WriteStreamAsync(Stream inputStream)
    {
      if (inputStream == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: inputStream.", default(Exception));

      await Task.Factory.StartNew(() =>
      {
        try
        {
          using (Stream outputStream = File.Create(this.filepath))
            inputStream.CopyTo(outputStream);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
        }
      });
    }

    /// <summary>
    /// Writes content to the underlying file as a byte array.
    /// </summary>
    /// <param name="bytes">Content to write to the underlying file as a byte array.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task WriteBytesAsync(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: bytes.", default(Exception));

      await Task.Factory.StartNew(() =>
      {
        try
        {
          File.WriteAllBytes(this.filepath, bytes);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (SecurityException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
        }
      });
    }

    /// <summary>
    /// Writes content to the underlying file as a text string.
    /// </summary>
    /// <param name="text">Content to write to the underlying file as a text string.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task WriteTextAsync(string text)
    {
      if (text == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: text.", default(Exception));

      await Task.Factory.StartNew(() =>
      {
        try
        {
          File.WriteAllText(this.filepath, text);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (SecurityException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
        }
      });
    }

    /// <summary>
    /// Deletes the underlying file.
    /// </summary>
    /// <exception cref="AccessDeniedException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="FileStorageException"></exception>
    public async Task DeleteAsync()
    {
      await Task.Factory.StartNew(() =>
      {
        if (!File.Exists(this.filepath))
          throw new FileNotFoundException($"File not found: \"{this.filepath}\".", null);

        try
        {
          File.Delete(this.filepath);
        }

        catch (UnauthorizedAccessException e)
        {
          throw new AccessDeniedException($"Access denied: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (System.IO.PathTooLongException e)
        {
          throw new PathTooLongException($"Path too long: \"{this.filepath}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
        }
      });
    }
  }
}