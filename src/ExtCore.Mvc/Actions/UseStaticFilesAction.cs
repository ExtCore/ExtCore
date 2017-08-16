// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExtCore.Mvc.Actions
{
  /// <summary>
  /// Implements the <see cref="IConfigureAction">IConfigureAction</see> interface and registers the
  /// static files middleware inside a web application's request pipeline.
  /// </summary>
  public class UseStaticFilesAction : IConfigureAction
  {
    /// <summary>
    /// Priority of the action. The actions will be executed in the order specified by the priority (from smallest to largest).
    /// </summary>
    public int Priority => 1000;

    /// <summary>
    /// Registers the static files middleware inside a web application's request pipeline.
    /// </summary>
    /// </summary>
    /// <param name="applicationBuilder">
    /// Will be provided by the ExtCore and might be used to configure a web application's request pipeline.
    /// </param>
    /// <param name="serviceProvider">
    /// Will be provided by the ExtCore and might be used to get any service that is registered inside the DI at this moment.
    /// </param>
    public void Execute(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
    {
      IOptions<StaticFileOptions> options = serviceProvider.GetService<IOptions<StaticFileOptions>>();

      applicationBuilder.UseStaticFiles(options?.Value);
    }
  }
}