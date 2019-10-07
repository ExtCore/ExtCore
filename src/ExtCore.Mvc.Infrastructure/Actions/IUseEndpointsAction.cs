// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Routing;

namespace ExtCore.Mvc.Infrastructure.Actions
{
  /// <summary>
  /// Describes an action that must be executed inside the UseEndpoints method and might be used by the extensions
  /// to configure the endpoints.
  /// </summary>
  public interface IUseEndpointsAction
  {
    /// <summary>
    /// Priority of the action. The actions will be executed in the order specified by the priority (from smallest to largest).
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Contains any code that must be executed inside the UseEndpoints method.
    /// </summary>
    /// <param name="endpointRouteBuilder">
    /// Will be provided by the ExtCore.Mvc and might be used to configure the endpoints.
    /// </param>
    /// <param name="serviceProvider">
    /// Will be provided by the ExtCore.Mvc and might be used to get any service that is registered inside the DI at this moment.
    /// </param>
    void Execute(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider serviceProvider);
  }
}