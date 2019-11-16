// Copyright © 2019 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using ExtCore.Infrastructure;
using ExtCore.Infrastructure.Actions;
using ExtCore.Mvc.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtCore.Mvc.Actions
{
  /// <summary>
  /// Implements the <see cref="IConfigureAction">IConfigureAction</see> interface and registers the
  /// endpoints middleware inside a web application's request pipeline.
  /// </summary>
  public class UseEndpointsAction : IConfigureAction
  {
    /// <summary>
    /// Priority of the action. The actions will be executed in the order specified by the priority (from smallest to largest).
    /// </summary>
    public int Priority => 11000;

    /// <summary>
    /// Registers the endpoints middleware inside a web application's request pipeline.
    /// </summary>
    /// <param name="applicationBuilder">
    /// Will be provided by the ExtCore and might be used to configure a web application's request pipeline.
    /// </param>
    /// <param name="serviceProvider">
    /// Will be provided by the ExtCore and might be used to get any service that is registered inside the DI at this moment.
    /// </param>
    public void Execute(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
    {
      applicationBuilder.UseEndpoints(
        endpointRouteBuilder =>
        {
          foreach (IUseEndpointsAction action in ExtensionManager.GetInstances<IUseEndpointsAction>().OrderBy(a => a.Priority))
          {
            ILogger logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger("ExtCore.Mvc");

            logger.LogInformation("Executing UseEndpoints action '{0}'", action.GetType().FullName);
            action.Execute(endpointRouteBuilder, serviceProvider);
          }
        }
      );
    }
  }
}