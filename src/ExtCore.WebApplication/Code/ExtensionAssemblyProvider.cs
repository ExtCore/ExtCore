// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtCore.Infrastructure;
using Microsoft.AspNet.Mvc.Infrastructure;

namespace ExtCore.WebApplication
{
  public class ExtensionAssemblyProvider : IAssemblyProvider
  {
    private readonly DefaultAssemblyProvider defaultAssemblyProvider;

    public ExtensionAssemblyProvider(DefaultAssemblyProvider defaultAssemblyProvider)
    {
      this.defaultAssemblyProvider = defaultAssemblyProvider;
    }

    public IEnumerable<Assembly> CandidateAssemblies
    {
      get
      {
        return this.defaultAssemblyProvider.CandidateAssemblies.Concat(ExtensionManager.Assemblies);
      }
    }
  }
}