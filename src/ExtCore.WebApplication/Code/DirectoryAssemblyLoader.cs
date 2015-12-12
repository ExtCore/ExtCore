// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Used code from https://github.com/aspnet/Entropy/blob/dev/samples/Runtime.CustomLoader/Program.cs

using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace ExtCore.WebApplication
{
  public class DirectoryAssemblyLoader : IAssemblyLoader
  {
    private readonly string path;
    private readonly IAssemblyLoadContext assemblyLoadContext;
    
    public DirectoryAssemblyLoader(string path, IAssemblyLoadContext assemblyLoadContext)
    {
      this.path = path;
      this.assemblyLoadContext = assemblyLoadContext;
    }

    public Assembly Load(AssemblyName assemblyName)
    {
      return this.assemblyLoadContext.LoadFile(Path.Combine(this.path, assemblyName + ".dll"));
    }

    public IntPtr LoadUnmanagedLibrary(string name)
    {
      throw new NotImplementedException();
    }
  }
}