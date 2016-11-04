// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtCore.WebApplication
{
  /// <summary>
  /// Represents default web application <c>Startup</c> class. Must be inherited in the derived web applications
  /// in order ExtCore basic functionality to work.
  /// </summary>
  public abstract class Startup
  {
    protected IServiceProvider serviceProvider;
    protected IAssemblyProvider assemblyProvider;
    protected IConfigurationRoot configurationRoot;
    protected ILogger<Startup> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup">Startup</see> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider that is used to get different services from the DI.</param>
    public Startup(IServiceProvider serviceProvider)
      : this(serviceProvider, new AssemblyProvider(serviceProvider))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup">Startup</see> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider that is used to get different services from the DI.</param>
    /// <param name="assemblyProvider">The assembly provider that is used to get the assemblies that should be involved in the ExtCore types discovery process.</param>
    public Startup(IServiceProvider serviceProvider, IAssemblyProvider assemblyProvider)
    {
      this.serviceProvider = serviceProvider;
      this.assemblyProvider = assemblyProvider;
      this.logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Startup>();
    }

    /// <summary>
    /// Discovers the assemblies, initializes extensions and services that are used by a web application.
    /// Also this method executes prioritized (defined in a specific order) actions (code fragments)
    /// from all the extensions, so each extension may execute some code within this method using the
    /// <c>ConfigureServicesActionsByPriorities</c> property of the <see cref="IExtension">IExtension</see> interface.
    /// </summary>
    /// <param name="services">The current set of the services configured in the DI.</param>
    public virtual void ConfigureServices(IServiceCollection services)
    {
      this.DiscoverAssemblies();

      foreach (IExtension extension in ExtensionManager.Extensions)
      {
        extension.SetServiceProvider(this.serviceProvider);
        extension.SetConfigurationRoot(this.configurationRoot);
      }

      foreach (Action<IServiceCollection> prioritizedConfigureServicesAction in this.GetPrioritizedConfigureServicesActions())
      {
        this.logger.LogInformation("Executing prioritized ConfigureServices action '{0}' of {1}", this.GetActionMethodInfo(prioritizedConfigureServicesAction));
        prioritizedConfigureServicesAction(services);
        this.RebuildServiceProvider(services);
      }
    }

    /// <summary>
    /// Executes prioritized (defined in a specific order) actions (code fragments)
    /// from all the extensions, so each extension may execute some code within this method using the
    /// <c>ConfigureActionsByPriorities</c> property of the <see cref="IExtension">IExtension</see> interface.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    public virtual void Configure(IApplicationBuilder applicationBuilder)
    {
      foreach (Action<IApplicationBuilder> prioritizedConfigureAction in this.GetPrioritizedConfigureActions())
      {
        this.logger.LogInformation("Executing prioritized Configure action '{0}' of {1}", this.GetActionMethodInfo(prioritizedConfigureAction));
        prioritizedConfigureAction(applicationBuilder);
      }
    }

    private void DiscoverAssemblies()
    {
      string extensionsPath = this.configurationRoot?["Extensions:Path"];
      IEnumerable<Assembly> assemblies = this.assemblyProvider.GetAssemblies(
        string.IsNullOrEmpty(extensionsPath) ?
          null : this.serviceProvider.GetService<IHostingEnvironment>().ContentRootPath + extensionsPath
      );

      ExtensionManager.SetAssemblies(assemblies);
    }

    private Action<IServiceCollection>[] GetPrioritizedConfigureServicesActions()
    {
      List<KeyValuePair<int, Action<IServiceCollection>>> configureServicesActionsByPriorities = new List<KeyValuePair<int, Action<IServiceCollection>>>();

      foreach (IExtension extension in ExtensionManager.Extensions)
        if (extension.ConfigureServicesActionsByPriorities != null)
          configureServicesActionsByPriorities.AddRange(extension.ConfigureServicesActionsByPriorities);

      return this.GetPrioritizedActions(configureServicesActionsByPriorities);
    }

    private void RebuildServiceProvider(IServiceCollection services)
    {
      this.serviceProvider = services.BuildServiceProvider();

      foreach (IExtension extension in ExtensionManager.Extensions)
        extension.SetServiceProvider(this.serviceProvider);
    }

    private Action<IApplicationBuilder>[] GetPrioritizedConfigureActions()
    {
      List<KeyValuePair<int, Action<IApplicationBuilder>>> configureActionsByPriorities = new List<KeyValuePair<int, Action<IApplicationBuilder>>>();

      foreach (IExtension extension in ExtensionManager.Extensions)
        if (extension.ConfigureActionsByPriorities != null)
          configureActionsByPriorities.AddRange(extension.ConfigureActionsByPriorities);

      return this.GetPrioritizedActions(configureActionsByPriorities);
    }

    private Action<T>[] GetPrioritizedActions<T>(IEnumerable<KeyValuePair<int, Action<T>>> actionsByPriorities)
    {
      return actionsByPriorities
        .OrderBy(actionByPriority => actionByPriority.Key)
        .Select(actionByPriority => actionByPriority.Value)
        .ToArray();
    }

    private string[] GetActionMethodInfo<T>(Action<T> action)
    {
      MethodInfo methodInfo = action.GetMethodInfo();

      return new string[] { methodInfo.Name, methodInfo.DeclaringType.FullName };
    }
  }
}