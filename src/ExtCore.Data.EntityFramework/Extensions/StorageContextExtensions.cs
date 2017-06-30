// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExtCore.Data.EntityFramework
{
  /// <summary>
  /// Contains the extension methods of the <see cref="IStorageContext">IStorageContext</see> interface.
  /// </summary>
  public static class StorageContextExtensions
  {
    /// <summary>
    /// Registers the entities from all the extensions inside the single Entity Framework storage context
    /// by finding all the implementations of the <see cref="IEntityRegistrar">IEntityRegistrar</see> interface.
    /// </summary>
    /// <param name="storageContext">The Entity Framework storage context.</param>
    /// <param name="modelBuilder">The Entity Framework model builder.</param>
    public static void RegisterEntities(this IStorageContext storageContext, ModelBuilder modelBuilder)
    {
      foreach (IEntityRegistrar entityRegistrar in ExtensionManager.GetInstances<IEntityRegistrar>())
        entityRegistrar.RegisterEntities(modelBuilder);
    }
  }
}