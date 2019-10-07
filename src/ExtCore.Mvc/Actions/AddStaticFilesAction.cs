// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using ExtCore.Infrastructure;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace ExtCore.Mvc.Actions
{
  /// <summary>
  /// Implements the <see cref="IConfigureServicesAction">IConfigureServicesAction</see> interface and
  /// creates and registers the composite file provider that contains resources from all the extensions.
  /// </summary>
  public class AddStaticFilesAction : IConfigureServicesAction
  {
    /// <summary>
    /// Priority of the action. The actions will be executed in the order specified by the priority (from smallest to largest).
    /// </summary>
    public int Priority => 1000;

    /// <summary>
    /// Creates and registers the composite file provider that contains resources from all the extensions.
    /// </summary>
    /// <param name="services">
    /// Will be provided by the ExtCore and might be used to register any service inside the DI.
    /// </param>
    /// <param name="serviceProvider">
    /// Will be provided by the ExtCore and might be used to get any service that is registered inside the DI at this moment.
    /// </param>
    public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
    {
      serviceProvider.GetService<IWebHostEnvironment>().WebRootFileProvider = this.CreateCompositeFileProvider(serviceProvider);
    }

    private IFileProvider CreateCompositeFileProvider(IServiceProvider serviceProvider)
    {
      IFileProvider[] fileProviders = new IFileProvider[] {
        serviceProvider.GetService<IWebHostEnvironment>().WebRootFileProvider
      };

      return new CompositeFileProvider(
        fileProviders.Concat(
          ExtensionManager.Assemblies.Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
        )
      );
    }
  }
}