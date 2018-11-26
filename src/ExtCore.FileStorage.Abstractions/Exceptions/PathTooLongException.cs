// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ExtCore.FileStorage
{
  /// <summary>
  /// Represents a path too long file storage exception.
  /// </summary>
  public class PathTooLongException : FileStorageException
  {
    public PathTooLongException() : base() { }
    public PathTooLongException(string message) : base(message) { }
    public PathTooLongException(string message, Exception innerException) : base(message, innerException) { }
  }
}