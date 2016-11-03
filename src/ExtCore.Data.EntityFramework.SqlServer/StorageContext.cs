// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExtCore.Data.EntityFramework.SqlServer
{
  /// <summary>
  /// Implements the <see cref="IStorageContext">IStorageContext</see> interface and represents SQL Server database.
  /// </summary>
  public class StorageContext : DbContext, IStorageContext
  {
    private string connectionString { get; set; }

    public StorageContext(string connectionString)
    {
      this.connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      optionsBuilder.UseSqlServer(this.connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      foreach (IModelRegistrar modelRegistrar in ExtensionManager.GetInstances<IModelRegistrar>(a => a.FullName.ToLower().Contains("entityframework.sqlserver")))
        modelRegistrar.RegisterModels(modelBuilder);
    }
  }
}