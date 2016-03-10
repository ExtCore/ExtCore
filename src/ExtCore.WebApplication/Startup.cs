// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using ExtCore.Infrastructure;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.FileProviders;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace ExtCore.WebApplication
{
  public class Startup
  {
    protected IConfigurationRoot configurationRoot;

    private string applicationBasePath;

    private IHostingEnvironment hostingEnvironment;
    private IAssemblyLoaderContainer assemblyLoaderContainer;
    private IAssemblyLoadContextAccessor assemblyLoadContextAccessor;
    private ILibraryManager libraryManager;

    public Startup(IHostingEnvironment hostingEnvironment, IApplicationEnvironment applicationEnvironment, IAssemblyLoaderContainer assemblyLoaderContainer, IAssemblyLoadContextAccessor assemblyLoadContextAccessor, ILibraryManager libraryManager)
    {
      this.hostingEnvironment = hostingEnvironment;
      this.applicationBasePath = applicationEnvironment.ApplicationBasePath;
      this.assemblyLoaderContainer = assemblyLoaderContainer;
      this.assemblyLoadContextAccessor = assemblyLoadContextAccessor;
      this.libraryManager = libraryManager;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
      string extensionsPath = this.configurationRoot["Extensions:Path"];            
      IEnumerable<Assembly> assemblies = AssemblyManager.GetAssemblies(
        Path.Combine(this.applicationBasePath.Substring(0, this.applicationBasePath.LastIndexOf("src")) + extensionsPath),
        this.assemblyLoaderContainer,
        this.assemblyLoadContextAccessor,
        this.libraryManager
      );

      ExtensionManager.SetAssemblies(assemblies);

      IFileProvider fileProvider = this.GetFileProvider(this.applicationBasePath);

      this.hostingEnvironment.WebRootFileProvider = fileProvider;
      services.AddCaching();
      services.AddSession();
      services.AddMvc().AddPrecompiledRazorViews(ExtensionManager.Assemblies.ToArray());
      services.Configure<RazorViewEngineOptions>(options =>
        {
          options.FileProvider = fileProvider;
        }
      );

      foreach (IExtension extension in ExtensionManager.Extensions)
      {
        extension.SetConfigurationRoot(this.configurationRoot);
        extension.ConfigureServices(services);
      }

      services.AddTransient<DefaultAssemblyProvider>();
      services.AddTransient<IAssemblyProvider, ExtensionAssemblyProvider>();
    }

    public virtual void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment)
    {
      applicationBuilder.UseSession();
      applicationBuilder.UseStaticFiles();

      foreach (IExtension extension in ExtensionManager.Extensions)
        extension.Configure(applicationBuilder);

      applicationBuilder.UseMvc(routeBuilder =>
        {
          routeBuilder.MapRoute(name: "Resource", template: "resource", defaults: new { controller = "Resource", action = "Index" });

          foreach (IExtension extension in ExtensionManager.Extensions)
            extension.RegisterRoutes(routeBuilder);
        }
      );
    }

    public IFileProvider GetFileProvider(string path)
    {
      IEnumerable<IFileProvider> fileProviders = new IFileProvider[] { new PhysicalFileProvider(path) };

      return new CompositeFileProvider(
        fileProviders.Concat(
          ExtensionManager.Assemblies.Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
        )
      );
    }
  }
}