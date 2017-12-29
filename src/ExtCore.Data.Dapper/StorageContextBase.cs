// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.Extensions.Options;

namespace ExtCore.Data.Dapper
{
  /// <summary>
  /// Implements the <see cref="IStorageContext">IStorageContext</see> interface and represents the physical storage
  /// with the Dapper Core as the ORM.
  /// </summary>
  public abstract class StorageContextBase : IStorageContext
  {
    /// <summary>
    /// The connection string that is used to connect to the physical storage.
    /// </summary>
    public string ConnectionString { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageContext">StorageContext</see> class.
    /// </summary>
    /// <param name="connectionStringProvider">The connection string that is used to connect to the physical storage.</param>
    public StorageContextBase(IOptions<StorageContextOptions> options)
    {
      this.ConnectionString = options.Value.ConnectionString;
    }
  }
}