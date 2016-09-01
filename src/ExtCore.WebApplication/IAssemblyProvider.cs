// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExtCore.WebApplication
{
  public interface IAssemblyProvider
  {
    void SetServiceProvider(IServiceProvider serviceProvider);
    IEnumerable<Assembly> GetAssemblies(string path);
  }
}