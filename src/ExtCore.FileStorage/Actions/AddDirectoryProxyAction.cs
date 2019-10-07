// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using ExtCore.FileStorage.Abstractions;
using ExtCore.Infrastructure;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtCore.FileStorage.Actions
{
  /// <summary>
  /// Implements the <see cref="IConfigureServicesAction">IConfigureServicesAction</see> interface and
  /// registers found implementation of the <see cref="IDirectoryProxy">IDirectoryProxy</see> interface inside the DI.
  /// </summary>
  public class AddDirectoryProxyAction : IConfigureServicesAction
  {
    /// <summary>
    /// Priority of the action. The actions will be executed in the order specified by the priority (from smallest to largest).
    /// </summary>
    public int Priority => 1000;

    /// <summary>
    /// Registers found implementation of the <see cref="IDirectoryProxy">IDirectoryProxy</see> interface inside the DI.
    /// </summary>
    /// <param name="services">
    /// Will be provided by the ExtCore and might be used to register any service inside the DI.
    /// </param>
    /// <param name="serviceProvider">
    /// Will be provided by the ExtCore and might be used to get any service that is registered inside the DI at this moment.
    /// </param>
    public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
    {
      Type type = ExtensionManager.GetImplementations<IDirectoryProxy>()?.FirstOrDefault(t => !t.GetTypeInfo().IsAbstract);

      if (type == null)
      {
        ILogger logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger("ExtCore.FileStorage");

        logger.LogError("Implementation of ExtCore.FileStorage.Abstractions.IDirectoryProxy not found");
        return;
      }

      services.AddScoped(typeof(IDirectoryProxy), type);
    }
  }
}