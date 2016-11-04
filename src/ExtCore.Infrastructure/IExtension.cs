// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExtCore.Infrastructure
{
  /// <summary>
  /// Describes an extension with the mechanism of executing prioritized (defined in a specific order)
  /// actions (code fragments) within the <c>ConfigureServices</c> and <c>Configure</c> methods of a web application
  /// <c>Startup</c> class.
  /// </summary>
  public interface IExtension
  {
    /// <summary>
    /// Gets the name of the extension.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the prioritized (defined in a specific order) actions (code fragments) that must be executed
    /// within the <c>ConfigureServices</c> method of a web application <c>Startup</c> class.
    /// Priority is set by the key, while the action is set by the value of the KeyValuePair. Before these actions are
    /// executed they will be merged with the actions of all other extensions according to the priorities.
    /// </summary>
    IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities { get; }

    /// <summary>
    /// Gets the prioritized (defined in a specific order) actions (code fragments) that must be executed
    /// within the <c>Configure</c> method of a web application <c>Startup</c> class.
    /// Priority is set by the key, while the action is set by the value of the KeyValuePair. Before these actions are
    /// executed they will be merged with the actions of all other extensions according to the priorities.
    /// </summary>
    IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities { get; }

    /// <summary>
    /// Sets the service provider that contains services currently registered inside the DI.
    /// Only these services will be available for the extension.
    /// </summary>
    /// <param name="serviceProvider">The service provider to set.</param>
    void SetServiceProvider(IServiceProvider serviceProvider);

    /// <summary>
    /// Sets the configuration root that is built in a web application <c>Startup</c> class constructor.
    /// This configuration root is only used to configure the extensions and will not be used to
    /// provide configuration properties to the controllers etc.
    /// </summary>
    /// <param name="configurationRoot">The configuration root to set.</param>
    void SetConfigurationRoot(IConfigurationRoot configurationRoot);
  }
}