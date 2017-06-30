// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using ExtCore.Infrastructure;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtCore.WebApplication.Extensions
{
  /// <summary>
  /// Contains the extension methods of the <see cref="IApplicationBuilder">IApplicationBuilder</see> interface.
  /// </summary>
  public static class ApplicationBuilderExtensions
  {
    /// <summary>
    /// Executes the Configure actions from all the extensions. It must be called inside the Configure method
    /// of the web application's Startup class in order ExtCore to work properly.
    /// </summary>
    /// <param name="applicationBuilder">
    /// The application builder passed to the Configure method of the web application's Startup class.
    /// </param>
    public static void UseExtCore(this IApplicationBuilder applicationBuilder)
    {
      ILogger logger = applicationBuilder.ApplicationServices.GetService<ILoggerFactory>().CreateLogger("ExtCore.WebApplication");

      foreach (IConfigureAction action in ExtensionManager.GetInstances<IConfigureAction>().OrderBy(a => a.Priority))
      {
        logger.LogInformation("Executing Configure action '{0}'", action.GetType().FullName);
        action.Execute(applicationBuilder, applicationBuilder.ApplicationServices);
      }
    }
  }
}