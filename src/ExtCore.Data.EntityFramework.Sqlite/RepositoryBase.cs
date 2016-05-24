// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Data.Models.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ExtCore.Data.EntityFramework.Sqlite
{
  public abstract class RepositoryBase<TEntity> : IRepository where TEntity : class, IEntity
  {
    protected StorageContext dbContext;
    protected DbSet<TEntity> dbSet;

    public void SetStorageContext(IStorageContext dbContext)
    {
      this.dbContext = dbContext as StorageContext;
      this.dbSet = this.dbContext.Set<TEntity>();
    }
  }
}