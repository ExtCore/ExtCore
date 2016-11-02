// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtCore.Infrastructure
{
  /// <summary>
  /// Implements the <see cref="IExtension">IExtension</see> interface and represents default extension behavior
  /// that might be overridden by the derived classes.
  /// </summary>
  public abstract class ExtensionBase : IExtension
  {
    protected IServiceProvider serviceProvider;
    protected IConfigurationRoot configurationRoot;
    protected ILogger<ExtensionBase> logger;

    public virtual string Name
    {
      get
      {
        return this.GetType().Name;
      }
    }

    public virtual IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
    {
      get
      {
        return null;
      }
    }

    public virtual IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities
    {
      get
      {
        return null;
      }
    }

    public virtual void SetServiceProvider(IServiceProvider serviceProvider)
    {
      this.serviceProvider = serviceProvider;
      this.logger = this.serviceProvider.GetService<ILoggerFactory>().CreateLogger<ExtensionBase>();
    }

    public virtual void SetConfigurationRoot(IConfigurationRoot configurationRoot)
    {
      this.configurationRoot = configurationRoot;
    }
  }
}