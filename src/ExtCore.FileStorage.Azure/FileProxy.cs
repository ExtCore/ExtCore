// Copyright © 2022 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ExtCore.FileStorage.Abstractions;

namespace ExtCore.FileStorage.Azure
{
  /// <summary>
  /// Implements the <see cref="IDirectoryProxy">IDirectoryProxy</see> interface and represents a file in a Azure Storage account.
  /// </summary>
  public class FileProxy : IFileProxy
  {
    private readonly string connectionString;
    private readonly string filepath;
    private readonly string containerName;
    private readonly string blobName;

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
    /// <param name="connectionString">The Azure Storage account connection string.</param>
    /// <param name="rootPath">The root path of the underlying file's relative one.</param>
    /// <param name="relativePath">The path of the underlying file relatively to the root one.</param>
    /// <param name="filename">The filename of the underlying file.</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public FileProxy(string connectionString, string rootPath, string relativePath, string filename)
    {
      if (connectionString == string.Empty)
        throw new ArgumentException($"Value can't be empty. Parameter name: connectionString.");

      if (connectionString == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: connectionString.", default(Exception));

      if (filename == string.Empty)
        throw new ArgumentException($"Value can't be empty. Parameter name: filename.");

      if (filename == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: filename.", default(Exception));

      this.connectionString = connectionString;
      this.RelativePath = RelativeUrl.Combine(relativePath);
      this.Filename = filename;
      this.filepath = RelativeUrl.Combine(rootPath, this.RelativePath, this.Filename);

      string[] urlSegments = filepath.Split('/');

      this.containerName = urlSegments.First();
      this.blobName = string.Join("/", urlSegments.Skip(1));
    }

    /// <summary>
    /// Checks if the underlying file exists.
    /// </summary>
    /// <returns>Returns a flag indicating if the underlying file exists.</returns>
    public async Task<bool> ExistsAsync()
    {
      try
      {
        BlobClient blobClient = await this.GetBlobClient();

        return await blobClient.ExistsAsync();
      }

      catch
      {
        return false;
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
    public async Task<Stream> ReadStreamAsync()
    {
      try
      {
        BlobClient blobClient = await this.GetBlobClient();

        return (await blobClient.DownloadStreamingAsync()).Value.Content;
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
        BlobClient blobClient = await this.GetBlobClient();
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();

        return downloadResult.Content.ToArray();
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
        BlobClient blobClient = await this.GetBlobClient();
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();

        return downloadResult.Content.ToString();
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
        BlobClient blobClient = await this.GetBlobClient();

        await blobClient.UploadAsync(inputStream);
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
        BlobClient blobClient = await this.GetBlobClient();

        await blobClient.UploadAsync(BinaryData.FromBytes(bytes));
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
        BlobClient blobClient = await this.GetBlobClient();

        await blobClient.UploadAsync(BinaryData.FromString(text));
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
        BlobClient blobClient = await this.GetBlobClient();

        await blobClient.DeleteAsync();
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.filepath}\". See inner exception for details.", e);
      }
    }

    private async Task<BlobClient> GetBlobClient()
    {
      BlobServiceClient blobServiceClient = new BlobServiceClient(this.connectionString);
      BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(this.containerName);

      await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
      return blobContainerClient.GetBlobClient(this.blobName);
    }
  }
}