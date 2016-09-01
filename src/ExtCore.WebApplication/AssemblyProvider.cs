// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;

namespace ExtCore.WebApplication
{
  public class AssemblyProvider : IAssemblyProvider
  {
    protected IServiceProvider serviceProvider;
    protected ILogger<AssemblyProvider> logger;

    public Func<Assembly, bool> IsCandidateAssembly { get; set; }
    public Func<Library, bool> IsCandidateCompilationLibrary { get; set; }

    public AssemblyProvider()
    {
      this.IsCandidateAssembly = assembly =>
        !assembly.FullName.StartsWith("Microsoft.") && !assembly.FullName.StartsWith("System.");
      this.IsCandidateCompilationLibrary = library =>
        library.Name != "NETStandard.Library" && !library.Name.StartsWith("Microsoft.") && !library.Name.StartsWith("System.");
    }

    public virtual void SetServiceProvider(IServiceProvider serviceProvider)
    {
      this.serviceProvider = serviceProvider;
      this.logger = this.serviceProvider.GetService<ILoggerFactory>().CreateLogger<AssemblyProvider>();
    }

    public IEnumerable<Assembly> GetAssemblies(string path)
    {
      List<Assembly> assemblies = new List<Assembly>();

      if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
      {
        this.logger.LogInformation("Discovering and loading assemblies from path '{0}'", path);

        foreach (string extensionPath in Directory.EnumerateFiles(path, "*.dll"))
        {
          Assembly assembly = null;

          try
          {
            assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(extensionPath);

            if (this.IsCandidateAssembly(assembly))
            {
              assemblies.Add(assembly);
              this.logger.LogInformation("Assembly '{0}' is discovered and loaded", assembly.FullName);
            }
          }

          catch (Exception e)
          {
            this.logger.LogWarning("Error loading assembly '{0}'", extensionPath);
            this.logger.LogInformation(e.ToString());
          }
        }
      }

      else
      {
        if (string.IsNullOrEmpty(path))
          this.logger.LogWarning("Discovering and loading assemblies from path skipped: path not provided", path);

        else this.logger.LogWarning("Discovering and loading assemblies from path '{0}' skipped: path not found", path);  
      }

      this.logger.LogInformation("Discovering and loading assemblies from DependencyContext");

      foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
      {
        if (this.IsCandidateCompilationLibrary(compilationLibrary))
        {
          Assembly assembly = null;

          try
          {
            assembly = Assembly.Load(new AssemblyName(compilationLibrary.Name));
            assemblies.Add(assembly);
            this.logger.LogInformation("Assembly '{0}' is discovered and loaded", assembly.FullName);
          }

          catch (Exception e)
          {
            this.logger.LogWarning("Error loading assembly '{0}'", compilationLibrary.Name);
            this.logger.LogInformation(e.ToString());
          }
        }
      }

      return assemblies;
    }
  }
}