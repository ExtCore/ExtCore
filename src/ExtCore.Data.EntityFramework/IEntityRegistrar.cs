// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace ExtCore.Data.EntityFramework
{
  /// <summary>
  /// Describes a mechanism of registering entities inside the Entity Framework storage context.
  /// </summary>
  public interface IEntityRegistrar
  {
    /// <summary>
    /// Registers entities inside the Entity Framework storage context.
    /// </summary>
    /// <param name="modelbuilder">The Entity Framework model builder.</param>
    void RegisterEntities(ModelBuilder modelbuilder);
  }
}