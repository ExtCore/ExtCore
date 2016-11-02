// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ExtCore.Mvc.Infrastructure
{
  /// <summary>
  /// Describes an MVC extension with the mechanism of executing prioritized (defined in a specific order)
  /// actions (code fragments) inside the <c>AddMvc</c> and <c>UseMvc</c> extension methods.
  /// </summary>
  public interface IExtension : ExtCore.Infrastructure.IExtension
  {
    IEnumerable<KeyValuePair<int, Action<IMvcBuilder>>> AddMvcActionsByPriorities { get; }
    IEnumerable<KeyValuePair<int, Action<IRouteBuilder>>> UseMvcActionsByPriorities { get; }
  }
}