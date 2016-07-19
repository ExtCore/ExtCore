// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace ExtCore.WebApplication
{
  public class Startup
  {
    protected IHostingEnvironment hostingEnvironment;
    protected ILoggerFactory loggerFactory;
    protected ILogger<Startup> logger;
    protected IConfigurationRoot configurationRoot;

    public Startup(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
    {
      this.hostingEnvironment = hostingEnvironment;
      this.loggerFactory = loggerFactory;
      this.logger = loggerFactory.CreateLogger<Startup>();
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
      this.DiscoverAssemblies();
      
      foreach (IExtension extension in ExtensionManager.Extensions)
        extension.SetConfigurationRoot(this.configurationRoot);

      foreach (Action<IServiceCollection> prioritizedConfigureServicesAction in this.GetPrioritizedConfigureServicesActions())
      {
        this.logger.LogInformation("Executing prioritized ConfigureServices action '{0}' of {1}", this.GetActionMethodInfo(prioritizedConfigureServicesAction));
        prioritizedConfigureServicesAction(services);
      }
    }

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
      string extensionsPath = this.configurationRoot["Extensions:Path"];
      IEnumerable<Assembly> assemblies = AssemblyManager.GetAssemblies(
        string.IsNullOrEmpty(extensionsPath) ? null : this.hostingEnvironment.ContentRootPath + extensionsPath
      );

      ExtensionManager.SetAssemblies(assemblies);

      foreach (Assembly assembly in ExtensionManager.Assemblies)
        this.logger.LogInformation("Assembly '{0}' is discovered, loaded and added to ExtensionManager", assembly.FullName);
    }

    private IFileProvider CreateCompositeFileProvider()
    {
      IFileProvider[] fileProviders = new IFileProvider[] {
        this.hostingEnvironment.WebRootFileProvider
      };

      return new CompositeFileProvider(
        fileProviders.Concat(
          ExtensionManager.Assemblies.Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
        )
      );
    }

    private Action<IServiceCollection>[] GetPrioritizedConfigureServicesActions()
    {
      List<KeyValuePair<int, Action<IServiceCollection>>> configureServicesActionsByPriorities = new Dictionary<int, Action<IServiceCollection>>()
      {
        [0] = this.AddStaticFiles,
        [100] = this.AddMvc
      }.ToList();

      foreach (IExtension extension in ExtensionManager.Extensions)
        if (extension.ConfigureServicesActionsByPriorities != null)
          configureServicesActionsByPriorities.AddRange(extension.ConfigureServicesActionsByPriorities);

      return this.GetPrioritizedActions(configureServicesActionsByPriorities);
    }

    private void AddStaticFiles(IServiceCollection services)
    {
      this.hostingEnvironment.WebRootFileProvider = this.CreateCompositeFileProvider();
    }

    private void AddMvc(IServiceCollection services)
    {
      IMvcBuilder mvcBuilder = services.AddMvc();

      foreach (Assembly assembly in ExtensionManager.Assemblies)
        mvcBuilder.AddApplicationPart(assembly);

      mvcBuilder.AddRazorOptions(
        o =>
        {
          foreach (Assembly assembly in ExtensionManager.Assemblies)
            o.FileProviders.Add(new EmbeddedFileProvider(assembly, assembly.GetName().Name));
        }
      );

      foreach (Action<IMvcBuilder> prioritizedAddMvcAction in this.GetPrioritizedAddMvcActions())
      {
        this.logger.LogInformation("Executing prioritized AddMvc action '{0}' of {1}", this.GetActionMethodInfo(prioritizedAddMvcAction));
        prioritizedAddMvcAction(mvcBuilder);
      }
    }

    private Action<IMvcBuilder>[] GetPrioritizedAddMvcActions()
    {
      List<KeyValuePair<int, Action<IMvcBuilder>>> addMvcActionsByPriorities = new List<KeyValuePair<int, Action<IMvcBuilder>>>();

      foreach (IExtension extension in ExtensionManager.Extensions)
        if (extension.AddMvcActionsByPriorities != null)
          addMvcActionsByPriorities.AddRange(extension.AddMvcActionsByPriorities);

      return this.GetPrioritizedActions(addMvcActionsByPriorities);
    }

    private Action<IApplicationBuilder>[] GetPrioritizedConfigureActions()
    {
      List<KeyValuePair<int, Action<IApplicationBuilder>>> configureActionsByPriorities = new Dictionary<int, Action<IApplicationBuilder>>()
      {
        [0] = this.UseStaticFiles,
        [100] = this.UseMvc
      }.ToList();

      foreach (IExtension extension in ExtensionManager.Extensions)
        if (extension.ConfigureActionsByPriorities != null)
          configureActionsByPriorities.AddRange(extension.ConfigureActionsByPriorities);

      return this.GetPrioritizedActions(configureActionsByPriorities);
    }

    private void UseStaticFiles(IApplicationBuilder applicationBuilder)
    {
      applicationBuilder.UseStaticFiles();
    }

    private void UseMvc(IApplicationBuilder applicationBuilder)
    {
      applicationBuilder.UseMvc(
        routeBuilder =>
        {
          foreach (Action<IRouteBuilder> prioritizedUseMvcAction in this.GetPrioritizedUseMvcActions())
          {
            this.logger.LogInformation("Executing prioritized UseMvc action '{0}' of {1}", this.GetActionMethodInfo(prioritizedUseMvcAction));
            prioritizedUseMvcAction(routeBuilder);
          }
        }
      );
    }

    private Action<IRouteBuilder>[] GetPrioritizedUseMvcActions()
    {
      List<KeyValuePair<int, Action<IRouteBuilder>>> useMvcActionsByPriorities = new List<KeyValuePair<int, Action<IRouteBuilder>>>();

      foreach (IExtension extension in ExtensionManager.Extensions)
        if (extension.UseMvcActionsByPriorities != null)
          useMvcActionsByPriorities.AddRange(extension.UseMvcActionsByPriorities);

      return this.GetPrioritizedActions(useMvcActionsByPriorities);
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