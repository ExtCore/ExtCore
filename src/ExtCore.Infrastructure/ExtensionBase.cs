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

    /// <summary>
    /// Gets the name of the extension.
    /// </summary>
    public virtual string Name
    {
      get
      {
        return this.GetType().Name;
      }
    }

    /// <summary>
    /// Gets the prioritized (defined in a specific order) actions (code fragments) that must be executed
    /// within the <c>ConfigureServices</c> method of a web application <c>Startup</c> class.
    /// Priority is set by the key, while the action is set by the value of the KeyValuePair. Before these actions are
    /// executed they will be merged with the actions of all other extensions according to the priorities.
    /// </summary>
    public virtual IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
    {
      get
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the prioritized (defined in a specific order) actions (code fragments) that must be executed
    /// within the <c>Configure</c> method of a web application <c>Startup</c> class.
    /// Priority is set by the key, while the action is set by the value of the KeyValuePair. Before these actions are
    /// executed they will be merged with the actions of all other extensions according to the priorities.
    /// </summary>
    public virtual IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities
    {
      get
      {
        return null;
      }
    }

    /// <summary>
    /// Sets the service provider that contains services currently registered inside the DI.
    /// Only these services will be available for the extension.
    /// </summary>
    /// <param name="serviceProvider">The service provider to set.</param>
    public virtual void SetServiceProvider(IServiceProvider serviceProvider)
    {
      this.serviceProvider = serviceProvider;
      this.logger = this.serviceProvider.GetService<ILoggerFactory>().CreateLogger<ExtensionBase>();
    }

    /// <summary>
    /// Sets the configuration root that is built in a web application <c>Startup</c> class constructor.
    /// This configuration root is only used to configure the extensions and will not be used to
    /// provide configuration properties to the controllers etc.
    /// </summary>
    /// <param name="configurationRoot">The configuration root to set.</param>
    public virtual void SetConfigurationRoot(IConfigurationRoot configurationRoot)
    {
      this.configurationRoot = configurationRoot;
    }
  }
}