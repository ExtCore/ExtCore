// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Builder;

namespace ExtCore.Infrastructure.Actions
{
  /// <summary>
  /// Describes an action that must be executed inside the Configure method of a web application's Startup class
  /// and might be used by the extensions to configure a web application's request pipeline.
  /// </summary>
  public interface IConfigureAction
  {
    /// <summary>
    /// Priority of the action. The actions will be executed in the order specified by the priority (from smallest to largest).
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Contains any code that must be executed inside the Configure method of the web application's Startup class.
    /// </summary>
    /// <param name="applicationBuilder">
    /// Will be provided by the ExtCore and might be used to configure a web application's request pipeline.
    /// </param>
    /// <param name="serviceProvider">
    /// Will be provided by the ExtCore and might be used to get any service that is registered inside the DI at this moment.
    /// </param>
    void Execute(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider);
  }
}