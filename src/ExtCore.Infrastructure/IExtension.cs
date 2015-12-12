// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExtCore.Infrastructure
{
  public interface IExtension
  {
    string Name { get; }

    void SetConfigurationRoot(IConfigurationRoot configurationRoot);
    void ConfigureServices(IServiceCollection services);
    void Configure(IApplicationBuilder applicationBuilder);
    void RegisterRoutes(IRouteBuilder routeBuilder);
  }
}