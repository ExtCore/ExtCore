// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace ExtCore.WebApplication
{
  public static class AssemblyManager
  {
    public static IEnumerable<Assembly> LoadAssemblies(string path, IAssemblyLoaderContainer assemblyLoaderContainer, IAssemblyLoadContextAccessor assemblyLoadContextAccessor)
    {
      List<Assembly> assemblies = new List<Assembly>();

      IAssemblyLoadContext assemblyLoadContext = assemblyLoadContextAccessor.Default;

      using (assemblyLoaderContainer.AddLoader(new DirectoryAssemblyLoader(path, assemblyLoadContext)))
      {
        foreach (string extensionPath in Directory.EnumerateFiles(path, "*.dll"))
        {
          string extensionFilename = Path.GetFileNameWithoutExtension(extensionPath);

          assemblies.Add(assemblyLoadContext.Load(extensionFilename));
        }
      }

      return assemblies;
    }
  }
}