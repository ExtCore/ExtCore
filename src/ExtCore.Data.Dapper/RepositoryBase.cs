// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Data.Entities.Abstractions;

namespace ExtCore.Data.Dapper
{
  /// <summary>
  /// Implements the <see cref="IRepository">IRepository</see> interface and represents default repository behavior.
  /// </summary>
  /// <typeparam name="TEntity">The entity type this repository operates.</typeparam>
  public abstract class RepositoryBase<TEntity> : IRepository where TEntity : class, IEntity
  {
    protected IStorageContext storageContext;
    protected string connectionString;

    /// <summary>
    /// Sets the Dapper storage context that represents the physical storage to work with.
    /// </summary>
    /// <param name="storageContext">The Dapper storage context to set.</param>
    public void SetStorageContext(IStorageContext storageContext)
    {
      this.storageContext = storageContext;
      this.connectionString = (storageContext as StorageContextBase).ConnectionString;
    }
  }
}