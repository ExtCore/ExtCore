// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExtCore.Infrastructure
{
  public interface IExtension
  {
    string Name { get; }
    IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities { get; }
    IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities { get; }

    void SetServiceProvider(IServiceProvider serviceProvider);
    void SetConfigurationRoot(IConfigurationRoot configurationRoot);
  }
}