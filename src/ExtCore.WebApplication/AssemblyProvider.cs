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
  /// <summary>
  /// Implements the <see cref="IAssemblyProvider">IAssemblyProvider</see> interface and represents
  /// default assembly provider that gets assemblies from a specific folder and web application dependencies
  /// with the ability to filter the discovered assemblies with the <c>IsCandidateAssembly</c> and
  /// <c>IsCandidateCompilationLibrary</c> predicates.
  /// </summary>
  public class AssemblyProvider : IAssemblyProvider
  {
    protected ILogger<AssemblyProvider> logger;

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
    /// Initializes a new instance of the <see cref="AssemblyProvider">AssemblyProvider</see> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider that is used to create a logger.</param>
    public AssemblyProvider(IServiceProvider serviceProvider)
    {
      this.logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<AssemblyProvider>();
      this.IsCandidateAssembly = assembly =>
        !assembly.FullName.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase) &&
        !assembly.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase);

      this.IsCandidateCompilationLibrary = library =>
        !string.Equals(library.Name, "NETStandard.Library", StringComparison.OrdinalIgnoreCase) &&
        !library.Name.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase) &&
        !library.Name.StartsWith("System.", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Discovers and then gets the discovered assemblies from a specific folder and web application dependencies.
    /// </summary>
    /// <param name="path">The extensions path of a web application.</param>
    /// <returns></returns>
    public IEnumerable<Assembly> GetAssemblies(string path)
    {
      List<Assembly> assemblies = new List<Assembly>();

      assemblies.AddRange(this.GetAssembliesFromPath(path));
      assemblies.AddRange(this.GetAssembliesFromDependencyContext());
      return assemblies;
    }

    private IEnumerable<Assembly> GetAssembliesFromPath(string path)
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

      return assemblies;
    }

    private IEnumerable<Assembly> GetAssembliesFromDependencyContext()
    {
      List<Assembly> assemblies = new List<Assembly>();

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