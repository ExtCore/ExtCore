// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace ExtCore.Mvc
{
  /// <summary>
  /// Implements the <see cref="IFileProvider">IFileProvider</see> interface and represents composite file provider
  /// that is built from the set of other file providers like <see cref="PhysicalFileProvider">PhysicalFileProvider</see>,
  /// <see cref="EmbeddedFileProvider">EmbeddedFileProvider</see> etc. It is used to make it possible to resolve
  /// files that are located in a file system, assemblies etc.
  /// </summary>
  public class CompositeFileProvider : IFileProvider
  {
    private readonly IEnumerable<IFileProvider> fileProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeFileProvider">CompositeFileProvider</see> class.
    /// </summary>
    /// <param name="fileProviders">The set of the file providers to look for the files in.</param>
    public CompositeFileProvider(IEnumerable<IFileProvider> fileProviders)
    {
      this.fileProviders = fileProviders;
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
      foreach (IFileProvider fileProvider in this.fileProviders)
      {
        IDirectoryContents directoryContents = fileProvider.GetDirectoryContents(subpath);

        if (directoryContents != null && directoryContents.Exists)
          return directoryContents;
      }

      return new NonexistentDirectoryContents();
    }

    public IFileInfo GetFileInfo(string subpath)
    {
      foreach (IFileProvider fileProvider in this.fileProviders)
      {
        IFileInfo fileInfo = fileProvider.GetFileInfo(subpath);

        if (fileInfo != null && fileInfo.Exists)
          return fileInfo;
      }

      return new NonexistentFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
      foreach (IFileProvider fileProvider in this.fileProviders)
      {
        IChangeToken changeToken = fileProvider.Watch(filter);

        if (changeToken != null)
          return changeToken;
      }

      return NonexistentChangeToken.Singleton;
    }
  }

  internal class NonexistentDirectoryContents : IDirectoryContents
  {
    public bool Exists
    {
      get
      {
        return false;
      }
    }

    public IEnumerator<IFileInfo> GetEnumerator()
    {
      return Enumerable.Empty<IFileInfo>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }

  internal class NonexistentFileInfo : IFileInfo
  {
    private readonly string name;

    public NonexistentFileInfo(string name)
    {
      this.name = name;
    }

    public bool Exists
    {
      get
      {
        return false;
      }
    }

    public bool IsDirectory
    {
      get
      {
        return false;
      }
    }

    public DateTimeOffset LastModified
    {
      get
      {
        return DateTimeOffset.MinValue;
      }
    }

    public long Length
    {
      get
      {
        return -1;
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public string PhysicalPath
    {
      get
      {
        return null;
      }
    }

    public Stream CreateReadStream()
    {
      throw new FileNotFoundException(this.name);
    }
  }

  internal class NonexistentChangeToken : IChangeToken
  {
    public static NonexistentChangeToken Singleton { get; } = new NonexistentChangeToken();

    public bool ActiveChangeCallbacks
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool HasChanged
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public IDisposable RegisterChangeCallback(Action<object> callback, object state)
    {
      throw new NotImplementedException();
    }
  }
}