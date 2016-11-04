// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ExtCore.Data.Abstractions
{
  /// <summary>
  /// Describes a repository for working with the underlying storage context.
  /// </summary>
  public interface IRepository
  {
    /// <summary>
    /// Sets the storage context to work with.
    /// </summary>
    /// <param name="storageContext">The storage context to set.</param>
    void SetStorageContext(IStorageContext storageContext);
  }
}