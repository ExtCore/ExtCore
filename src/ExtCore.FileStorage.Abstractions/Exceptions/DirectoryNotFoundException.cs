// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ExtCore.FileStorage
{
  /// <summary>
  /// Represents a directory not found file storage exception.
  /// </summary>
  public class DirectoryNotFoundException : FileStorageException
  {
    public DirectoryNotFoundException() : base() { }
    public DirectoryNotFoundException(string message) : base(message) { }
    public DirectoryNotFoundException(string message, Exception innerException) : base(message, innerException) { }
  }
}