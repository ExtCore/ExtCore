// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ExtCore.Data.EntityFramework
{
  /// <summary>
  /// Represents Entity Framework storage context options.
  /// </summary>
  public class StorageContextOptions
  {
    /// <summary>
    /// The connection string that is used to connect to the physical storage.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// The assembly name where migrations are maintained for this context.
    /// </summary>
    public string MigrationsAssembly { get; set; }
  }
}