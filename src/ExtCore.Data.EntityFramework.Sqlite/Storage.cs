// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;

namespace ExtCore.Data.EntityFramework.Sqlite
{
  /// <summary>
  /// Implements the <see cref="IStorage">IStorage</see> interface and represents implementation of the
  /// Unit of Work design pattern with the mechanism of getting the repositories to work with the underlying
  /// SQLite database storage context and committing the changes made by all the repositories.
  /// </summary>
  public class Storage : IStorage
  {
    public static string ConnectionString { get; set; }

    public StorageContext StorageContext { get; private set; }

    public Storage()
    {
      this.StorageContext = new StorageContext(Storage.ConnectionString);
    }

    public TRepository GetRepository<TRepository>() where TRepository : IRepository
    {
      TRepository repository = ExtensionManager.GetInstance<TRepository>(a => a.FullName.ToLower().Contains("entityframework.sqlite"));

      repository.SetStorageContext(this.StorageContext);
      return repository;
    }

    public void Save()
    {
      this.StorageContext.SaveChanges();
    }
  }
}