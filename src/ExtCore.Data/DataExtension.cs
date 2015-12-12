// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExtCore.Data
{
  public class DataExtension : IExtension
  {
    private IConfigurationRoot configurationRoot;

    public string Name
    {
      get
      {
        return "Data Extension";
      }
    }

    public void SetConfigurationRoot(IConfigurationRoot configurationRoot)
    {
      this.configurationRoot = configurationRoot;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      Type type = this.GetIStorageImplementationType();

      if (type != null)
      {
        PropertyInfo connectionStringPropertyInfo = type.GetProperty("ConnectionString");

        if (connectionStringPropertyInfo != null)
          connectionStringPropertyInfo.SetValue(null, this.configurationRoot["Data:DefaultConnection:ConnectionString"]);

        PropertyInfo assembliesPropertyInfo = type.GetProperty("Assemblies");

        if (assembliesPropertyInfo != null)
          assembliesPropertyInfo.SetValue(null, ExtensionManager.Assemblies);

        services.AddScoped(typeof(IStorage), type);
      }
    }

    public void Configure(IApplicationBuilder applicationBuilder)
    {
    }

    public void RegisterRoutes(IRouteBuilder routeBuilder)
    {
    }

    private Type GetIStorageImplementationType()
    {
      foreach (Assembly assembly in ExtensionManager.Assemblies.Where(a => a.FullName.Contains("Data")))
        foreach (Type type in assembly.GetTypes())
          if (typeof(IStorage).IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
            return type;

      return null;
    }
  }
}