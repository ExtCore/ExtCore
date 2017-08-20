// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using ExtCore.Infrastructure;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtCore.WebApplication.Extensions
{
  /// <summary>
  /// Contains the extension methods of the <see cref="IServiceCollection">IServiceCollection</see> interface.
  /// </summary>
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Discovers the assemblies and executes the ConfigureServices actions from all the extensions.
    /// It must be called inside the ConfigureServices method of the web application's Startup class
    /// in order ExtCore to work properly.
    /// </summary>
    /// <param name="services">
    /// The services collection passed to the ConfigureServices method of the web application's Startup class.
    /// </param>
    public static void AddExtCore(this IServiceCollection services)
    {
      services.AddExtCore(null);
    }

    /// <summary>
    /// Discovers the assemblies and executes the ConfigureServices actions from all the extensions.
    /// It must be called inside the ConfigureServices method of the web application's Startup class
    /// in order ExtCore to work properly.
    /// </summary>
    /// <param name="services">
    /// The services collection passed to the ConfigureServices method of the web application's Startup class.
    /// </param>
    /// <param name="extensionsPath">The absolute extensions path.</param>
    public static void AddExtCore(this IServiceCollection services, string extensionsPath)
    {
      services.AddExtCore(extensionsPath, false, new DefaultAssemblyProvider(services.BuildServiceProvider()));
    }

    /// <summary>
    /// Discovers the assemblies and executes the ConfigureServices actions from all the extensions.
    /// It must be called inside the ConfigureServices method of the web application's Startup class
    /// in order ExtCore to work properly.
    /// </summary>
    /// <param name="services">
    /// The services collection passed to the ConfigureServices method of the web application's Startup class.
    /// </param>
    /// <param name="extensionsPath">The absolute extensions path.</param>
    /// <param name="includingSubpaths">Determines whether the assemblies must be loaded from the subfolders recursively.</param>
    public static void AddExtCore(this IServiceCollection services, string extensionsPath, bool includingSubpaths)
    {
      services.AddExtCore(extensionsPath, includingSubpaths, new DefaultAssemblyProvider(services.BuildServiceProvider()));
    }

    /// <summary>
    /// Discovers the assemblies and executes the ConfigureServices actions from all the extensions.
    /// It must be called inside the ConfigureServices method of the web application's Startup class
    /// in order ExtCore to work properly.
    /// </summary>
    /// <param name="services">
    /// The services collection passed to the ConfigureServices method of the web application's Startup class.
    /// </param>
    /// <param name="extensionsPath">The absolute extensions path.</param>
    /// <param name="assemblyProvider">The assembly provider that is used to discover and load the assemblies.</param>
    public static void AddExtCore(this IServiceCollection services, string extensionsPath, IAssemblyProvider assemblyProvider)
    {
      services.AddExtCore(extensionsPath, false, assemblyProvider);
    }

    /// <summary>
    /// Discovers the assemblies and executes the ConfigureServices actions from all the extensions.
    /// It must be called inside the ConfigureServices method of the web application's Startup class
    /// in order ExtCore to work properly.
    /// </summary>
    /// <param name="services">
    /// The services collection passed to the ConfigureServices method of the web application's Startup class.
    /// </param>
    /// <param name="extensionsPath">The absolute extensions path.</param>
    /// <param name="includingSubpaths">Determines whether the assemblies must be loaded from the subfolders recursively.</param>
    /// <param name="assemblyProvider">The assembly provider that is used to discover and load the assemblies.</param>
    public static void AddExtCore(this IServiceCollection services, string extensionsPath, bool includingSubpaths, IAssemblyProvider assemblyProvider)
    {
      ServiceCollectionExtensions.DiscoverAssemblies(assemblyProvider, extensionsPath, includingSubpaths);

      IServiceProvider serviceProvider = services.BuildServiceProvider();
      ILogger logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger("ExtCore.WebApplication");

      foreach (IConfigureServicesAction action in ExtensionManager.GetInstances<IConfigureServicesAction>().OrderBy(a => a.Priority))
      {
        logger.LogInformation("Executing ConfigureServices action '{0}'", action.GetType().FullName);
        action.Execute(services, serviceProvider);
        serviceProvider = services.BuildServiceProvider();
      }
    }

    private static void DiscoverAssemblies(IAssemblyProvider assemblyProvider, string extensionsPath, bool includingSubpaths)
    {
      ExtensionManager.SetAssemblies(assemblyProvider.GetAssemblies(extensionsPath, includingSubpaths));
    }
  }
}