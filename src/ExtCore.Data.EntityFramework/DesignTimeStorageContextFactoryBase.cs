// Copyright © 2017 David Noreña, Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ExtCore.Data.Abstractions;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace ExtCore.Data.EntityFramework
{
  /// <summary>
  /// Implements the <see cref="IDesignTimeDbContextFactory{TContext}"/> interface and represents the factory for providing
  /// the registered inside the DI storage context service to the Entity Framework Core tools (such as Migrations).
  /// Inherit from this class and call the Initialize method in your web application after the <see cref="StorageContextOptions"/> class
  /// is configured in order the Entity Framework Core tools (such as Migrations) to work.
  /// </summary>
  /// <typeparam name="T">The storage context type this factory creates.</typeparam>
  public abstract class DesignTimeStorageContextFactoryBase<T> : IDesignTimeDbContextFactory<T> where T : StorageContextBase
  {
    /// <summary>
    /// The storage context service from the DI.
    /// </summary>
    public static T StorageContext { get; set; }

    /// <summary>
    /// Gets the storage context service set by the Initialize method.
    /// </summary>
    /// <param name="args"></param>
    /// <returns>Storage context service set by the Initialize method.</returns>
    public T CreateDbContext(string[] args)
    {
      return DesignTimeStorageContextFactoryBase<T>.StorageContext;
    }

    /// <summary>
    /// Gets the storage context service from the DI and then sets it to the StorageContext property.
    /// Call this method inside the Startup.ConfigureServices one after the <see cref="StorageContextOptions"/> class
    /// is configured.
    /// </summary>
    /// <param name="serviceProvider">The service provider to get the storage context service.</param>
    public static void Initialize(IServiceProvider serviceProvider)
    {
      DesignTimeStorageContextFactoryBase<T>.StorageContext = serviceProvider.GetService<IStorageContext>() as T;
    }
  }
}