// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ExtCore.Data.Dapper
{
  /// <summary>
  /// Represents Dapper storage context options.
  /// </summary>
  public class StorageContextOptions
  {
    /// <summary>
    /// The connection string that is used to connect to the physical storage.
    /// </summary>
    public string ConnectionString { get; set; }
  }
}