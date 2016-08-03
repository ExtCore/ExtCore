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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace ExtCore.Mvc
{
  public class MvcExtension : ExtCore.Mvc.Infrastructure.ExtensionBase
  {
    public override IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
    {
      get
      {
        return new Dictionary<int, Action<IServiceCollection>>()
        {
          [0] = this.AddStaticFiles,
          [10000] = this.AddMvc
        };
      }
    }

    public override IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities
    {
      get
      {
        return new Dictionary<int, Action<IApplicationBuilder>>()
        {
          [0] = this.UseStaticFiles,
          [10000] = this.UseMvc
        };
      }
    }

    private void AddStaticFiles(IServiceCollection services)
    {
      this.serviceProvider.GetService<IHostingEnvironment>().WebRootFileProvider = this.CreateCompositeFileProvider();
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
        if (extension is ExtCore.Mvc.Infrastructure.IExtension)
          if ((extension as ExtCore.Mvc.Infrastructure.IExtension).AddMvcActionsByPriorities != null)
            addMvcActionsByPriorities.AddRange((extension as ExtCore.Mvc.Infrastructure.IExtension).AddMvcActionsByPriorities);

      return this.GetPrioritizedActions(addMvcActionsByPriorities);
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
        if (extension is ExtCore.Mvc.Infrastructure.IExtension)
          if ((extension as ExtCore.Mvc.Infrastructure.IExtension).UseMvcActionsByPriorities != null)
            useMvcActionsByPriorities.AddRange((extension as ExtCore.Mvc.Infrastructure.IExtension).UseMvcActionsByPriorities);

      return this.GetPrioritizedActions(useMvcActionsByPriorities);
    }

    private IFileProvider CreateCompositeFileProvider()
    {
      IFileProvider[] fileProviders = new IFileProvider[] {
        this.serviceProvider.GetService<IHostingEnvironment>().WebRootFileProvider
      };

      return new CompositeFileProvider(
        fileProviders.Concat(
          ExtensionManager.Assemblies.Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
        )
      );
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