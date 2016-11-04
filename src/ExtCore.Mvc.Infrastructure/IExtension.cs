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
  /// actions (code fragments) within the <c>AddMvc</c> and <c>UseMvc</c> extension methods while they are executed
  /// by the ExtCore.Mvc extension to configure the MVC.
  /// </summary>
  public interface IExtension : ExtCore.Infrastructure.IExtension
  {
    /// <summary>
    /// Gets the prioritized (defined in a specific order) actions (code fragments) that must be executed
    /// within the <c>AddMvc</c> extension method while it is executed by the ExtCore.Mvc extension to configure the MVC.
    /// Priority is set by the key, while the action is set by the value of the KeyValuePair. Before these actions are
    /// executed they will be merged with the actions of all other extensions according to the priorities.
    /// </summary>
    IEnumerable<KeyValuePair<int, Action<IMvcBuilder>>> AddMvcActionsByPriorities { get; }

    /// <summary>
    /// Gets the prioritized (defined in a specific order) actions (code fragments) that must be executed
    /// within the <c>UseMvc</c> extension method while it is executed by the ExtCore.Mvc extension to configure the MVC.
    /// Priority is set by the key, while the action is set by the value of the KeyValuePair. Before these actions are
    /// executed they will be merged with the actions of all other extensions according to the priorities.
    /// </summary>
    IEnumerable<KeyValuePair<int, Action<IRouteBuilder>>> UseMvcActionsByPriorities { get; }
  }
}