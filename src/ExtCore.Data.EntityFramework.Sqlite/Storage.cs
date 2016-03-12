// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;

namespace ExtCore.Data.EntityFramework.Sqlite
{
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