// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtCore.Data.Abstractions;
using Microsoft.Data.Entity;

namespace ExtCore.Data.EntityFramework.Sqlite
{
  public class StorageContext : DbContext, IStorageContext
  {
    private string connectionString { get; set; }
    private IEnumerable<Assembly> assemblies { get; set; }

    public StorageContext(string connectionString, IEnumerable<Assembly> assemblies)
    {
      this.connectionString = connectionString;
      this.assemblies = assemblies;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      optionsBuilder.UseSqlite(this.connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      foreach (Assembly assembly in Storage.Assemblies.Where(a => a.FullName.Contains("EntityFramework.Sqlite")))
      {
        foreach (Type type in assembly.GetTypes())
        {
          if (typeof(IModelRegistrar).IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
          {
            IModelRegistrar modelRegistrar = (IModelRegistrar)Activator.CreateInstance(type);

            modelRegistrar.RegisterModels(modelBuilder);
          }
        }
      }
    }
  }
}