// Copyright © 2022 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ExtCore.FileStorage.Abstractions;

namespace ExtCore.FileStorage.Azure
{
  /// <summary>
  /// Implements the <see cref="IDirectoryProxy">IDirectoryProxy</see> interface and represents a directory in a Azure Storage account.
  /// </summary>
  public class DirectoryProxy : IDirectoryProxy
  {
    private readonly string connectionString;
    private readonly string rootPath;
    private readonly string path;
    private readonly string containerName;
    private readonly string prefix;

    /// <summary>
    /// The path of the underlying directory relatively to the root one.
    /// </summary>
    public string RelativePath { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryProxy">DirectoryProxy</see> class.
    /// </summary>
    /// <param name="connectionString">The Azure Storage account connection string.</param>
    /// <param name="rootPath">The root path of the underlying directory's relative one.</param>
    /// <param name="relativePath">The path of the underlying directory relatively to the root one.</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public DirectoryProxy(string connectionString, string rootPath, string relativePath)
    {
      if (connectionString == string.Empty)
        throw new ArgumentException($"Value can't be empty. Parameter name: connectionString.");

      if (connectionString == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: connectionString.", default(Exception));

      this.connectionString = connectionString;
      this.rootPath = RelativeUrl.Combine(rootPath);
      this.RelativePath = RelativeUrl.Combine(relativePath);
      this.path = RelativeUrl.Combine(this.rootPath, this.RelativePath);

      string[] urlSegments = path.Split('/');

      this.containerName = urlSegments.First();
      this.prefix = string.Join("/", urlSegments.Skip(1));
    }

    /// <summary>
    /// Checks if the underlying directory exists.
    /// </summary>
    /// <returns>Returns a flag indicating if the underlying directory exists.</returns>
    public async Task<bool> ExistsAsync()
    {
      try
      {
        BlobContainerClient blobContainerClient = this.GetBlobContainerClient();
        IAsyncEnumerable<Page<BlobHierarchyItem>> pages = blobContainerClient.GetBlobsByHierarchyAsync(prefix: this.prefix).AsPages();

        await foreach (Page<BlobHierarchyItem> page in pages)
          return page.Values.Count != 0;
      }

      catch { }

      return false;
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
      throw new NotSupportedException();
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
      throw new NotSupportedException();
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
      try
      {
        BlobContainerClient blobContainerClient = this.GetBlobContainerClient();

        if (string.IsNullOrEmpty(this.prefix))
        {
          await blobContainerClient.DeleteAsync();
          return;
        }

        string prefix = this.prefix + "/";
        IAsyncEnumerable<Page<BlobHierarchyItem>> pages = blobContainerClient.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: recursive ? null : "/").AsPages();

        await foreach (Page<BlobHierarchyItem> page in pages)
        {
          foreach (BlobHierarchyItem blobItem in page.Values)
          {
            if (blobItem.IsBlob)
            {
              BlobClient blobClient = await this.GetBlobClient(blobItem.Blob);

              await blobClient.DeleteAsync();
            }
          }
        }
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
      }
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
      try
      {
        IList<IDirectoryProxy> directoryProxies = new List<IDirectoryProxy>();
        BlobContainerClient blobContainerClient = this.GetBlobContainerClient();
        string prefix = string.IsNullOrEmpty(this.prefix) ? null : this.prefix + "/";
        IAsyncEnumerable<Page<BlobHierarchyItem>> pages = blobContainerClient.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: "/").AsPages();

        await foreach (Page<BlobHierarchyItem> page in pages)
          foreach (BlobHierarchyItem blobItem in page.Values)
            if (blobItem.IsPrefix)
              directoryProxies.Add(new DirectoryProxy(this.connectionString, this.rootPath, RelativeUrl.Combine(this.RelativePath, RelativeUrl.Combine(blobItem.Prefix).Split('/').Last())));

        return directoryProxies;
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
      }
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
      try
      {
        IList<IFileProxy> fileProxies = new List<IFileProxy>();
        BlobContainerClient blobContainerClient = this.GetBlobContainerClient();
        string prefix = string.IsNullOrEmpty(this.prefix) ? null : this.prefix + "/";
        IAsyncEnumerable<Page<BlobHierarchyItem>> pages = blobContainerClient.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: "/").AsPages();

        await foreach (Page<BlobHierarchyItem> page in pages)
          foreach (BlobHierarchyItem blobItem in page.Values)
            if (blobItem.IsBlob)
              fileProxies.Add(new FileProxy(this.connectionString, this.rootPath, this.RelativePath, blobItem.Blob.Name.Split('/').Last()));

        return fileProxies;
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
      }
    }

    private BlobContainerClient GetBlobContainerClient()
    {
      BlobServiceClient blobServiceClient = new BlobServiceClient(this.connectionString);

      return blobServiceClient.GetBlobContainerClient(this.containerName);
    }

    private async Task<BlobClient> GetBlobClient(BlobItem blobItem)
    {
      BlobContainerClient blobContainerClient = this.GetBlobContainerClient();

      await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
      return blobContainerClient.GetBlobClient(blobItem.Name);
    }
  }
}