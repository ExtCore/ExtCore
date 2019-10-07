// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;

namespace ExtCore.WebApplication
{
  /// <summary>
  /// Implements the <see cref="IAssemblyProvider">IAssemblyProvider</see> interface and represents
  /// default assembly provider that gets assemblies from a specific path and web application dependencies
  /// with the ability to filter the discovered assemblies with the IsCandidateAssembly and
  /// IsCandidateCompilationLibrary predicates.
  /// </summary>
  public class DefaultAssemblyProvider : IAssemblyProvider
  {
    protected ILogger logger;

    /// <summary>
    /// Gets or sets the predicate that is used to filter discovered assemblies from a specific folder
    /// before thay have been added to the resulting assemblies set.
    /// </summary>
    public Func<Assembly, bool> IsCandidateAssembly { get; set; }

    /// <summary>
    /// Gets or sets the predicate that is used to filter discovered libraries from a web application dependencies
    /// before thay have been added to the resulting assemblies set.
    /// </summary>
    public Func<Library, bool> IsCandidateCompilationLibrary { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultAssemblyProvider">AssemblyProvider</see> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider that is used to create a logger.</param>
    public DefaultAssemblyProvider(IServiceProvider serviceProvider)
    {
      this.logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger("ExtCore.WebApplication");
      this.IsCandidateAssembly = assembly =>
        !assembly.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase) &&
        !assembly.FullName.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase);

      this.IsCandidateCompilationLibrary = library =>
        !library.Name.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase) &&
        !library.Name.StartsWith("netstandard", StringComparison.OrdinalIgnoreCase) &&
        !library.Name.StartsWith("System", StringComparison.OrdinalIgnoreCase) &&
        !library.Name.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase) &&
        !library.Name.StartsWith("WindowsBase", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Discovers and then gets the discovered assemblies from a specific folder and web application dependencies.
    /// </summary>
    /// <param name="path">The extensions path of a web application.</param>
    /// <param name="includingSubpaths">
    /// Determines whether a web application will discover and then get the discovered assemblies from the subfolders
    /// of a specific folder recursively.
    /// </param>
    /// <returns>The discovered and loaded assemblies.</returns>
    public IEnumerable<Assembly> GetAssemblies(string path, bool includingSubpaths)
    {
      List<Assembly> assemblies = new List<Assembly>();

      this.GetAssembliesFromDependencyContext(assemblies);
      this.GetAssembliesFromPath(assemblies, path, includingSubpaths);
      return assemblies;
    }

    private void GetAssembliesFromDependencyContext(List<Assembly> assemblies)
    {
      this.logger.LogInformation("Discovering and loading assemblies from DependencyContext");

      foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
      {
        if (this.IsCandidateCompilationLibrary(compilationLibrary))
        {
          Assembly assembly = null;

          try
          {
            assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(compilationLibrary.Name));

            if (!assemblies.Any(a => string.Equals(a.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
            {
              assemblies.Add(assembly);
              this.logger.LogInformation("Assembly '{0}' is discovered and loaded", assembly.FullName);
            }
          }

          catch (Exception e)
          {
            this.logger.LogWarning("Error loading assembly '{0}'", compilationLibrary.Name);
            this.logger.LogWarning(e.ToString());
          }
        }
      }
    }

    private void GetAssembliesFromPath(List<Assembly> assemblies, string path, bool includingSubpaths)
    {
      if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
      {
        this.logger.LogInformation("Discovering and loading assemblies from path '{0}'", path);

        foreach (string extensionPath in Directory.EnumerateFiles(path, "*.dll"))
        {
          Assembly assembly = null;

          try
          {
            assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(extensionPath);

            if (this.IsCandidateAssembly(assembly) && !assemblies.Any(a => string.Equals(a.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
            {
              assemblies.Add(assembly);
              this.logger.LogInformation("Assembly '{0}' is discovered and loaded", assembly.FullName);
            }
          }

          catch (Exception e)
          {
            this.logger.LogWarning("Error loading assembly '{0}'", extensionPath);
            this.logger.LogWarning(e.ToString());
          }
        }

        if (includingSubpaths)
          foreach (string subpath in Directory.GetDirectories(path))
            this.GetAssembliesFromPath(assemblies, subpath, includingSubpaths);
      }

      else
      {
        if (string.IsNullOrEmpty(path))
          this.logger.LogWarning("Discovering and loading assemblies from path skipped: path not provided", path);

        else this.logger.LogWarning("Discovering and loading assemblies from path '{0}' skipped: path not found", path);
      }
    }
  }
}