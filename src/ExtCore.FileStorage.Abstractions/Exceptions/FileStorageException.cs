// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ExtCore.FileStorage
{
  /// <summary>
  /// Represents a generic file storage exception.
  /// </summary>
  public class FileStorageException : Exception
  {
    public FileStorageException() : base() { }
    public FileStorageException(string message) : base(message) { }
    public FileStorageException(string message, Exception innerException) : base(message, innerException) { }
  }
}