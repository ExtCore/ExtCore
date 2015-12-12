// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExtCore.Infrastructure
{
  public static class ExtensionManager
  {
    private static IEnumerable<Assembly> assemblies;
    private static IEnumerable<IExtension> extensions;

    public static IEnumerable<Assembly> Assemblies
    {
      get
      {
        if (ExtensionManager.assemblies == null)
          throw new InvalidOperationException("Assemblies not set");

        return ExtensionManager.assemblies;
      }
    }

    public static IEnumerable<IExtension> Extensions
    {
      get
      {
        if (ExtensionManager.assemblies == null)
          throw new InvalidOperationException("Assemblies not set");

        if (ExtensionManager.extensions == null)
        {
          List<IExtension> extensions = new List<IExtension>();

          foreach (Assembly assembly in ExtensionManager.assemblies)
          {
            foreach (Type type in assembly.GetTypes())
            {
              if (typeof(IExtension).IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
              {
                IExtension extension = (IExtension)Activator.CreateInstance(type);

                extensions.Add(extension);
              }
            }
          }

          ExtensionManager.extensions = extensions;
        }

        return ExtensionManager.extensions;
      }
    }

    public static void SetAssemblies(IEnumerable<Assembly> assemblies)
    {
      ExtensionManager.assemblies = assemblies;
    }
  }
}