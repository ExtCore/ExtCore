// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ExtCore.Data.EntityFramework.PostgreSql
{
  /// <summary>
  /// Implements the <see cref="IStorageContext">IStorageContext</see> interface and represents PostgreSQL database
  /// with the Entity Framework Core as the ORM.
  /// </summary>
  public class StorageContext : StorageContextBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StorageContext">StorageContext</see> class.
    /// </summary>
    /// <param name="options">The options that are used to connect to the PostgreSQL database.</param>
    public StorageContext(IOptions<StorageContextOptions> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);

      if (string.IsNullOrEmpty(this.MigrationsAssembly))
        optionsBuilder.UseNpgsql(this.ConnectionString);

      else optionsBuilder.UseNpgsql(
        this.ConnectionString,
        options =>
        {
          options.MigrationsAssembly(this.MigrationsAssembly);
        }
      );
    }
  }
}