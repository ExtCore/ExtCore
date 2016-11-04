// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ExtCore.Mvc.Infrastructure
{
  /// <summary>
  /// Implements the <see cref="IExtension">IExtension</see> interface and represents default MVC extension behavior
  /// that might be overridden by the derived classes.
  /// </summary>
  public abstract class ExtensionBase : ExtCore.Infrastructure.ExtensionBase, IExtension
  {
    /// <summary>
    /// Gets the prioritized (defined in a specific order) actions (code fragments) that must be executed
    /// within the <c>AddMvc</c> extension method while it is executed by the ExtCore.Mvc extension to configure the MVC.
    /// Priority is set by the key, while the action is set by the value of the KeyValuePair. Before these actions are
    /// executed they will be merged with the actions of all other extensions according to the priorities.
    /// </summary>
    public virtual IEnumerable<KeyValuePair<int, Action<IMvcBuilder>>> AddMvcActionsByPriorities
    {
      get
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the prioritized (defined in a specific order) actions (code fragments) that must be executed
    /// within the <c>UseMvc</c> extension method while it is executed by the ExtCore.Mvc extension to configure the MVC.
    /// Priority is set by the key, while the action is set by the value of the KeyValuePair. Before these actions are
    /// executed they will be merged with the actions of all other extensions according to the priorities.
    /// </summary>
    public virtual IEnumerable<KeyValuePair<int, Action<IRouteBuilder>>> UseMvcActionsByPriorities
    {
      get
      {
        return null;
      }
    }
  }
}