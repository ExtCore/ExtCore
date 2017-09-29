﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ExtCore.Data.EntityFramework.Sqlite
{
  /// <summary>
  /// Implements the <see cref="IStorageContext">IStorageContext</see> interface and represents SQLite database
  /// with the Entity Framework Core as the ORM.
  /// </summary>
  public class StorageContext : StorageContextBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StorageContext">StorageContext</see> class.
    /// </summary>
    /// <param name="connectionString">The connection string that is used to connect to the SQLite database.</param>
    public StorageContext(IOptions<StorageContextOptions> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      optionsBuilder.UseSqlite(this.ConnectionString, b => b.MigrationsAssembly(this.MigrationsAssembly));
    }
  }
}