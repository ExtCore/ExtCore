// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using ExtCore.FileStorage.Abstractions;

namespace ExtCore.FileStorage.Dropbox
{
  /// <summary>
  /// Implements the <see cref="IDirectoryProxy">IDirectoryProxy</see> interface and represents a file in a Dropbox account.
  /// </summary>
  public class FileProxy : IFileProxy
  {
    private readonly string accessToken;
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
    /// <param name="accessToken">The Dropbox's account access token.</param>
    /// <param name="rootPath">The root path of the underlying file's relative one.</param>
    /// <param name="relativePath">The path of the underlying file relatively to the root one.</param>
    /// <param name="filename">The filename of the underlying file.</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public FileProxy(string accessToken, string rootPath, string relativePath, string filename)
    {
      if (accessToken == string.Empty)
        throw new ArgumentException($"Value can't be empty. Parameter name: accessToken.");

      if (accessToken == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: accessToken.", default(Exception));

      if (filename == string.Empty)
        throw new ArgumentException($"Value can't be empty. Parameter name: filename.");

      if (filename == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: filename.", default(Exception));

      this.accessToken = accessToken;
      this.rootPath = rootPath;
      this.RelativePath = relativePath;
      this.Filename = filename;
      this.filepath = RelativeUrl.Combine(this.rootPath, this.RelativePath, this.Filename);
    }

    /// <summary>
    /// Checks if the underlying file exists.
    /// </summary>
    /// <returns>Returns a flag indicating if the underlying file exists.</returns>
    public async Task<bool> ExistsAsync()
    {
      try
      {
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
        {
          Metadata metadata = await dropboxClient.Files.GetMetadataAsync(this.filepath);

          return metadata.IsFile;
        }
      }

      catch { return false; }
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
    public async Task<Stream> ReadStreamAsync()
    {
      try
      {
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
        using (IDownloadResponse<FileMetadata> response = await dropboxClient.Files.DownloadAsync(this.filepath))
          return await response.GetContentAsStreamAsync();
      }

      catch (ApiException<DownloadError> e)
      {
        if (e.ErrorResponse.IsPath)
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);

        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }
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
      try
      {
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
        using (IDownloadResponse<FileMetadata> response = await dropboxClient.Files.DownloadAsync(this.filepath))
          return await response.GetContentAsByteArrayAsync();
      }

      catch (ApiException<DownloadError> e)
      {
        if (e.ErrorResponse.IsPath)
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);

        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }
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
      try
      {
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
        using (IDownloadResponse<FileMetadata> response = await dropboxClient.Files.DownloadAsync(this.filepath))
          return await response.GetContentAsStringAsync();
      }

      catch (ApiException<DownloadError> e)
      {
        if (e.ErrorResponse.IsPath)
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);

        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }
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
      try
      {
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
          await dropboxClient.Files.UploadAsync(this.filepath, WriteMode.Overwrite.Instance, body: inputStream);
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }
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
      try
      {
        using (MemoryStream inputStream = new MemoryStream(bytes))
          await this.WriteStreamAsync(inputStream);
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }
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
      try
      {
        await this.WriteBytesAsync(Encoding.UTF8.GetBytes(text));
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }
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
      try
      {
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
          await dropboxClient.Files.DeleteV2Async(this.filepath);
      }

      catch (ApiException<ListFolderError> e)
      {
        if (e.ErrorResponse.IsPath)
          throw new DirectoryNotFoundException($"Directory not found: \"{this.filepath}\". See inner exception for details.", e);

        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }
    }
  }
}