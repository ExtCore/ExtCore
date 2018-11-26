// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using ExtCore.FileStorage.Abstractions;

namespace ExtCore.FileStorage.Dropbox
{
  /// <summary>
  /// Implements the <see cref="IDirectoryProxy">IDirectoryProxy</see> interface and represents a directory in a Dropbox account.
  /// </summary>
  public class DirectoryProxy : IDirectoryProxy
  {
    private readonly string accessToken;
    private readonly string rootPath;
    private readonly string path;

    /// <summary>
    /// The path of the underlying directory relatively to the root one.
    /// </summary>
    public string RelativePath { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryProxy">DirectoryProxy</see> class.
    /// </summary>
    /// <param name="accessToken">The Dropbox's account access token.</param>
    /// <param name="rootPath">The root path of the underlying directory's relative one.</param>
    /// <param name="relativePath">The path of the underlying directory relatively to the root one.</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public DirectoryProxy(string accessToken, string rootPath, string relativePath)
    {
      if (accessToken == string.Empty)
        throw new ArgumentException($"Value can't be empty. Parameter name: accessToken.");

      if (accessToken == null)
        throw new ArgumentNullException($"Value can't be null. Parameter name: accessToken.", default(Exception));

      this.accessToken = accessToken;
      this.rootPath = RelativeUrl.Combine(rootPath);
      this.RelativePath = RelativeUrl.Combine(relativePath);
      this.path = RelativeUrl.Combine(this.rootPath, this.RelativePath);

      if (string.Equals(this.path, "/"))
        this.path = string.Empty;
    }

    /// <summary>
    /// Checks if the underlying directory exists.
    /// </summary>
    /// <returns>Returns a flag indicating if the underlying directory exists.</returns>
    public async Task<bool> ExistsAsync()
    {
      try
      {
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
        {
          Metadata metadata = await dropboxClient.Files.GetMetadataAsync(this.path);

          return metadata.IsFolder;
        }
      }

      catch { return false; }
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
      try
      {
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
          await dropboxClient.Files.CreateFolderV2Async(this.path);
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
      }
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

      try
      {
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
          await dropboxClient.Files.MoveV2Async(this.path, RelativeUrl.Combine(this.rootPath, destinationRelativePath));
      }

      catch (ApiException<RelocationError> e)
      {
        if (e.ErrorResponse.IsFromLookup)
          throw new DirectoryNotFoundException($"Directory not found: \"{this.path}\". See inner exception for details.", e);

        throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
      }

      catch (Exception e)
      {
        throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
      }
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
        using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
          await dropboxClient.Files.DeleteV2Async(this.path);
      }

      catch (ApiException<DeleteError> e)
      {
        if (e.ErrorResponse.IsPathLookup)
          throw new DirectoryNotFoundException($"Directory not found: \"{this.path}\". See inner exception for details.", e);

        throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
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
      using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
      {
        IList<IDirectoryProxy> directoryProxies = new List<IDirectoryProxy>();

        try
        {
          foreach (Metadata metadata in (await dropboxClient.Files.ListFolderAsync(this.path)).Entries.Where(m => m.IsFolder))
            directoryProxies.Add(new DirectoryProxy(this.accessToken, this.rootPath, metadata.PathDisplay.Substring(this.rootPath.Length)));
        }

        catch (ApiException<ListFolderError> e)
        {
          if (e.ErrorResponse.IsPath)
            throw new DirectoryNotFoundException($"Directory not found: \"{this.path}\". See inner exception for details.", e);

          throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
        }

        return directoryProxies;
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
      using (DropboxClient dropboxClient = new DropboxClient(this.accessToken))
      {
        IList<IFileProxy> fileProxies = new List<IFileProxy>();

        try
        {
          foreach (Metadata metadata in (await dropboxClient.Files.ListFolderAsync(this.path)).Entries.Where(m => m.IsFile))
          {
            string relativePath = metadata.PathDisplay;

            relativePath = relativePath.Substring(this.rootPath.Length);

            if (relativePath.Contains("/"))
              relativePath = relativePath.Remove(relativePath.LastIndexOf("/"));

            fileProxies.Add(new FileProxy(this.accessToken, this.rootPath, relativePath, metadata.Name));
          }
        }

        catch (ApiException<ListFolderError> e)
        {
          if (e.ErrorResponse.IsPath)
            throw new DirectoryNotFoundException($"Directory not found: \"{this.path}\". See inner exception for details.", e);

          throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
        }

        catch (Exception e)
        {
          throw new FileStorageException($"Generic file storage exception: \"{this.path}\". See inner exception for details.", e);
        }

        return fileProxies;
      }
    }
  }
}