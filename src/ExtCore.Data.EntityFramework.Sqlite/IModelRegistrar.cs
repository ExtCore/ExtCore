// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace ExtCore.Data.EntityFramework.Sqlite
{
  /// <summary>
  /// Describes a mechanism of registering entities inside the SQLite storage context.
  /// </summary>
  public interface IModelRegistrar
  {
    /// <summary>
    /// Registers entities inside the SQLite storage context.
    /// </summary>
    /// <param name="modelbuilder">The Entity Framework model builder.</param>
    void RegisterModels(ModelBuilder modelbuilder);
  }
}