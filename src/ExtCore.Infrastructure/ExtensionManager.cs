// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
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
        if (ExtensionManager.extensions == null)
          ExtensionManager.extensions = ExtensionManager.GetInstances<IExtension>();

        return ExtensionManager.extensions;
      }
    }

    public static void SetAssemblies(IEnumerable<Assembly> assemblies)
    {
      ExtensionManager.assemblies = assemblies;
    }

    public static Type GetImplementation<T>()
    {
      return ExtensionManager.GetImplementation<T>(null);
    }

    public static Type GetImplementation<T>(Func<Assembly, bool> predicate)
    {
      return ExtensionManager.GetImplementations<T>(predicate).FirstOrDefault();
    }

    public static IEnumerable<Type> GetImplementations<T>()
    {
      return ExtensionManager.GetImplementations<T>(null);
    }

    public static IEnumerable<Type> GetImplementations<T>(Func<Assembly, bool> predicate)
    {
      List<Type> implementations = new List<Type>();

      foreach (Assembly assembly in ExtensionManager.GetAssemblies(predicate))
        foreach (Type type in assembly.GetTypes())
          if (typeof(T).GetTypeInfo().IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
            implementations.Add(type);

      return implementations;
    }

    public static T GetInstance<T>()
    {
      return ExtensionManager.GetInstance<T>(null);
    }

    public static T GetInstance<T>(Func<Assembly, bool> predicate)
    {
      return ExtensionManager.GetInstances<T>(predicate).FirstOrDefault();
    }

    public static IEnumerable<T> GetInstances<T>()
    {
      return ExtensionManager.GetInstances<T>(null);
    }

    public static IEnumerable<T> GetInstances<T>(Func<Assembly, bool> predicate)
    {
      List<T> instances = new List<T>();

      foreach (Type implementation in ExtensionManager.GetImplementations<T>())
      {
        if (!implementation.GetTypeInfo().IsAbstract)
        {
          T instance = (T)Activator.CreateInstance(implementation);

          instances.Add(instance);
        }
      }

      return instances;
    }

    private static IEnumerable<Assembly> GetAssemblies(Func<Assembly, bool> predicate)
    {
      if (predicate == null)
        return ExtensionManager.Assemblies;

      return ExtensionManager.Assemblies.Where(predicate);
    }
  }
}