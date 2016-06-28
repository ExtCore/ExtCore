// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExtCore.Infrastructure
{
  public interface IExtension
  {
    string Name { get; }
    IDictionary<int, Action<IRouteBuilder>> RouteRegistrarsByPriorities { get; }

    void SetConfigurationRoot(IConfigurationRoot configurationRoot);
    void ConfigureServices(IServiceCollection services);
    void Configure(IApplicationBuilder applicationBuilder);
  }
}