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
    /// <summary>
    /// Gets or sets the connection string that is used to connect to the SQLite database.
    /// </summary>
    public static string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the storage context that represents the SQLite database.
    /// </summary>
    public StorageContext StorageContext { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Storage">Storage</see> class.
    /// </summary>
    public Storage()
    {
      this.StorageContext = new StorageContext(Storage.ConnectionString);
    }

    /// <summary>
    /// Gets a repository of the given type.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <returns></returns>
    public TRepository GetRepository<TRepository>() where TRepository : IRepository
    {
      TRepository repository = ExtensionManager.GetInstance<TRepository>(a => a.FullName.ToLower().Contains("entityframework.sqlite"));

      repository.SetStorageContext(this.StorageContext);
      return repository;
    }

    /// <summary>
    /// Commits the changes made by all the repositories.
    /// </summary>
    public void Save()
    {
      this.StorageContext.SaveChanges();
    }
  }
}