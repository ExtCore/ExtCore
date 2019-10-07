// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using ExtCore.Infrastructure;
using ExtCore.Infrastructure.Actions;
using ExtCore.Mvc.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtCore.Mvc.Actions
{
  /// <summary>
  /// Implements the <see cref="IConfigureServicesAction">IConfigureServicesAction</see> interface and
  /// registers the MVC services inside the DI.
  /// </summary>
  public class AddMvcAction : IConfigureServicesAction
  {
    /// <summary>
    /// Priority of the action. The actions will be executed in the order specified by the priority (from smallest to largest).
    /// </summary>
    public int Priority => 10000;

    /// <summary>
    /// Registers the MVC services inside the DI.
    /// </summary>
    /// <param name="services">
    /// Will be provided by the ExtCore and might be used to register any service inside the DI.
    /// </param>
    /// <param name="serviceProvider">
    /// Will be provided by the ExtCore and might be used to get any service that is registered inside the DI at this moment.
    /// </param>
    public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
    {
      IMvcBuilder mvcBuilder = services.AddMvc();

      foreach (Assembly assembly in ExtensionManager.Assemblies)
        mvcBuilder.AddApplicationPart(assembly);

      foreach (IAddMvcAction action in ExtensionManager.GetInstances<IAddMvcAction>().OrderBy(a => a.Priority))
      {
        ILogger logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger("ExtCore.Mvc");

        logger.LogInformation("Executing AddMvc action '{0}'", action.GetType().FullName);
        action.Execute(mvcBuilder, serviceProvider);
      }
    }
  }
}