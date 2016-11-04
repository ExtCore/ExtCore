// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtCore.Data
{
  /// <summary>
  /// Overrides the <see cref="ExtensionBase">ExtensionBase</see> class and defines the <c>ConfigureServices</c> method
  /// prioritized action for registering existing implementation of the <see cref="IStorage">IStorage</see> interface
  /// inside the DI.
  /// </summary>
  public class DataExtension : ExtensionBase
  {
    /// <summary>
    /// Defines one prioritized action with priority = 1000 that looks for the implementation of the
    /// <see cref="IStorage">IStorage</see> interface and registers it inside the DI if found.
    /// </summary>
    public override IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
    {
      get
      {
        return new Dictionary<int, Action<IServiceCollection>>()
        {
          [1000] = services =>
          {
            Type type = ExtensionManager.GetImplementation<IStorage>(a => a.FullName.ToLower().Contains("data"));

            if (type == null)
            {
              this.logger.LogError("Implementation of ExtCore.Data.Abstractions.IStorage not found");
              return;
            }

            string connectionString = this.configurationRoot?["Data:DefaultConnection:ConnectionString"];

            if (string.IsNullOrEmpty(connectionString))
            {
              this.logger.LogError("Connection string is not provided");
              return;
            }

            type.GetProperty("ConnectionString").SetValue(null, connectionString);
            services.AddScoped(typeof(IStorage), type);
          }
        };
      }
    }
  }
}