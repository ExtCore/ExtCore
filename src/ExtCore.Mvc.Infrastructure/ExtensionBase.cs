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
    public virtual IEnumerable<KeyValuePair<int, Action<IMvcBuilder>>> AddMvcActionsByPriorities
    {
      get
      {
        return null;
      }
    }

    public virtual IEnumerable<KeyValuePair<int, Action<IRouteBuilder>>> UseMvcActionsByPriorities
    {
      get
      {
        return null;
      }
    }
  }
}