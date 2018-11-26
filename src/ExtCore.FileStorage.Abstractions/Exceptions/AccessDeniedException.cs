// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ExtCore.FileStorage
{
  /// <summary>
  /// Represents an access denied file storage exception.
  /// </summary>
  public class AccessDeniedException : FileStorageException
  {
    public AccessDeniedException() : base() { }
    public AccessDeniedException(string message) : base(message) { }
    public AccessDeniedException(string message, Exception innerException) : base(message, innerException) { }
  }
}