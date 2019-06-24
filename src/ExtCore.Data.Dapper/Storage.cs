// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;

namespace ExtCore.Data.Dapper
{
  /// <summary>
  /// Implements the <see cref="IStorage">IStorage</see> interface and represents implementation of the
  /// Unit of Work design pattern with the mechanism of getting the repositories to work with the underlying
  /// Dapper storage context and committing the changes made by all the repositories.
  /// </summary>
  public class Storage : IStorage
  {
    /// <summary>
    /// Gets the Dapper storage context.
    /// </summary>
    public IStorageContext StorageContext { get; private set; }

    public Storage(IStorageContext storageContext)
    {
      this.StorageContext = storageContext;
    }

    /// <summary>
    /// Gets a repository of the given type.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <returns></returns>
    public TRepository GetRepository<TRepository>() where TRepository : IRepository
    {
      TRepository repository = ExtensionManager.GetInstance<TRepository>();

      if (repository != null)
        repository.SetStorageContext(this.StorageContext);

      return repository;
    }

    /// <summary>
    /// Commits the changes made by all the repositories.
    /// </summary>
    public void Save()
    {
    }

    /// <summary>
    /// Asynchronously commits the changes made by all the repositories.
    /// </summary>
    public async Task SaveAsync()
    {
    }
  }
}