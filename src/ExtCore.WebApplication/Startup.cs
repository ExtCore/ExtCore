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

namespace ExtCore.WebApplication
{
  public class Startup
  {
    protected IConfigurationRoot configurationRoot;

    private IHostingEnvironment hostingEnvironment;

    public Startup(IHostingEnvironment hostingEnvironment)
    {
      this.hostingEnvironment = hostingEnvironment;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
      this.DiscoverAssemblies();
      this.hostingEnvironment.WebRootFileProvider = this.CreateCompositeFileProvider();

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

      foreach (IExtension extension in ExtensionManager.Extensions)
      {
        extension.SetConfigurationRoot(this.configurationRoot);
        extension.ConfigureServices(services);
      }
    }

    public virtual void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment)
    {
      applicationBuilder.UseStaticFiles();

      foreach (IExtension extension in ExtensionManager.Extensions)
        extension.Configure(applicationBuilder);

      applicationBuilder.UseMvc(routeBuilder =>
        {
          foreach (KeyValuePair<int, List<Action<IRouteBuilder>>> routeRegistrarSetByPriority in this.GetRouteRegistrarSetsByPriorities().OrderBy(routeRegistrarSetByPriority => routeRegistrarSetByPriority.Key))
            foreach (Action<IRouteBuilder> routeRegistrar in routeRegistrarSetByPriority.Value)
              routeRegistrar(routeBuilder);
        }
      );
    }

    private void DiscoverAssemblies()
    {
      string extensionsPath = this.configurationRoot["Extensions:Path"];

      IEnumerable<Assembly> assemblies = AssemblyManager.GetAssemblies(
        string.IsNullOrEmpty(extensionsPath) ? null : this.hostingEnvironment.ContentRootPath + extensionsPath
      );

      ExtensionManager.SetAssemblies(assemblies);
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

    private Dictionary<int, List<Action<IRouteBuilder>>> GetRouteRegistrarSetsByPriorities()
    {
      Dictionary<int, List<Action<IRouteBuilder>>> routeRegistrarSetsByPriorities = new Dictionary<int, List<Action<IRouteBuilder>>>();

      foreach (IExtension extension in ExtensionManager.Extensions)
      {
        if (extension.RouteRegistrarsByPriorities != null)
        {
          foreach (KeyValuePair<int, Action<IRouteBuilder>> routeRegistrarByPriority in extension.RouteRegistrarsByPriorities)
          {
            if (!routeRegistrarSetsByPriorities.ContainsKey(routeRegistrarByPriority.Key))
              routeRegistrarSetsByPriorities.Add(routeRegistrarByPriority.Key, new List<Action<IRouteBuilder>>());

            routeRegistrarSetsByPriorities[routeRegistrarByPriority.Key].Add(routeRegistrarByPriority.Value);
          }
        }
      }

      return routeRegistrarSetsByPriorities;
    }
  }
}