﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ExtCore.Data.EntityFramework
{
  /// <summary>
  /// Implements the <see cref="IStorageContext">IStorageContext</see> interface and represents the physical storage
  /// with the Entity Framework Core as the ORM.
  /// </summary>
  public abstract class StorageContextBase : DbContext, IStorageContext
  {
    /// <summary>
    /// The connection string that is used to connect to the physical storage.
    /// </summary>
    public string ConnectionString { get; private set; }

    /// <summary>
    /// The assembly name that is used to design entity framework migrations.
    /// </summary>
    public string MigrationsAssembly { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageContext">StorageContext</see> class.
    /// </summary>
    /// <param name="connectionStringProvider">The connection string that is used to connect to the physical storage.</param>
    public StorageContextBase(IOptions<StorageContextOptions> options)
    {
      this.ConnectionString = options.Value.ConnectionString;
      this.MigrationsAssembly = options.Value.MigrationsAssembly;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      this.RegisterEntities(modelBuilder);
    }
  }
}