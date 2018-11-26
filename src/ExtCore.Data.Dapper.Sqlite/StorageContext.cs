// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Options;

namespace ExtCore.Data.Dapper.Sqlite
{
  /// <summary>
  /// Implements the <see cref="IStorageContext">IStorageContext</see> interface and represents SQLite database
  /// with the Dapper as the ORM.
  /// </summary>
  public class StorageContext : StorageContextBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StorageContext">StorageContext</see> class.
    /// </summary>
    /// <param name="options">The options that are used to connect to the SQLite database.</param>
    public StorageContext(IOptions<StorageContextOptions> options)
      : base(options)
    {
    }
  }
}