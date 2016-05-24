// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace ExtCore.WebApplication
{
  public class Startup
  {
    protected IConfigurationRoot configurationRoot;

    private IHostingEnvironment hostingEnvironment;

    public Startup(IHostingEnvironment hostingEnvironment)
    {
      this.hostingEnvironment = hostingEnvironment;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
      this.DiscoverAssemblies();
      this.hostingEnvironment.WebRootFileProvider = this.CreateCompositeFileProvider();
      this.AddMvcServices(services);
      
      foreach (IExtension extension in ExtensionManager.Extensions)
      {
        extension.SetConfigurationRoot(this.configurationRoot);
        extension.ConfigureServices(services);
      }
    }

    public virtual void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment)
    {
      applicationBuilder.UseStaticFiles();

      foreach (IExtension extension in ExtensionManager.Extensions)
        extension.Configure(applicationBuilder);

      applicationBuilder.UseMvc(routeBuilder =>
        {
          foreach (IExtension extension in ExtensionManager.Extensions)
            extension.RegisterRoutes(routeBuilder);
        }
      );
    }

    private void DiscoverAssemblies()
    {
      string extensionsPath = this.hostingEnvironment.ContentRootPath + this.configurationRoot["Extensions:Path"];
      IEnumerable<Assembly> assemblies = AssemblyManager.GetAssemblies(extensionsPath);

      ExtensionManager.SetAssemblies(assemblies);
    }

    private void AddMvcServices(IServiceCollection services)
    {
      IMvcBuilder mvcBuilder = services.AddMvc();
      List<MetadataReference> metadataReferences = new List<MetadataReference>();

      foreach (Assembly assembly in ExtensionManager.Assemblies)
      {
        mvcBuilder.AddApplicationPart(assembly);
        metadataReferences.Add(MetadataReference.CreateFromFile(assembly.Location));
      }

      mvcBuilder.AddRazorOptions(
        o =>
        {
          foreach (Assembly assembly in ExtensionManager.Assemblies)
            o.FileProviders.Add(new EmbeddedFileProvider(assembly, assembly.GetName().Name));

          Action<RoslynCompilationContext> previous = o.CompilationCallback;

          o.CompilationCallback = c =>
          {
            if (previous != null)
            {
              previous(c);
            }

            c.Compilation = c.Compilation.AddReferences(metadataReferences);
          };
        }
      );
    }

    private IFileProvider CreateCompositeFileProvider()
    {
      IEnumerable<IFileProvider> fileProviders = new IFileProvider[] {
        this.hostingEnvironment.WebRootFileProvider
      };

      return new CompositeFileProvider(
        fileProviders.Concat(
          ExtensionManager.Assemblies.Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
        )
      );
    }
  }
}