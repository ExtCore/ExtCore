// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

namespace ExtCore.WebApplication
{
  public class AssemblyProvider : IAssemblyProvider
  {
    public Func<Assembly, bool> IsCandidateAssembly { get; set; }
    public Func<Library, bool> IsCandidateCompilationLibrary { get; set; }

    public AssemblyProvider()
    {
      this.IsCandidateAssembly = assembly =>
        !assembly.FullName.StartsWith("Microsoft.") && !assembly.FullName.StartsWith("System.");
      this.IsCandidateCompilationLibrary = library =>
        library.Name != "NETStandard.Library" && !library.Name.StartsWith("Microsoft.") && !library.Name.StartsWith("System.");
    }

    public IEnumerable<Assembly> GetAssemblies(string path)
    {
      List<Assembly> assemblies = new List<Assembly>();

      if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
      {
        foreach (string extensionPath in Directory.EnumerateFiles(path, "*.dll"))
        {
          Assembly assembly = null;

          try
          {
            assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(extensionPath);

            if (this.IsCandidateAssembly(assembly))
              assemblies.Add(assembly);
          }

          catch
          {
            // TODO: Add errors logging
          }
        }
      }

      foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
      {
        if (this.IsCandidateCompilationLibrary(compilationLibrary))
        {
          Assembly assembly = null;

          try
          {
            assembly = Assembly.Load(new AssemblyName(compilationLibrary.Name));
            assemblies.Add(assembly);
          }

          catch
          {
            // TODO: Add errors logging
          }
        }
      }

      return assemblies;
    }
  }
}