// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExtCore.Infrastructure
{
  /// <summary>
  /// Represents assembly cache with the mechanism of getting implementations or instances of a given type.
  /// Also it allows to get all the instances of the <see cref="IExtension">IExtension</see> interface
  /// with the additional cache. This is the global access point to the types discovered by the ExtCore.
  /// </summary>
  public static class ExtensionManager
  {
    private static IEnumerable<Assembly> assemblies;
    private static IEnumerable<IExtension> extensions;

    /// <summary>
    /// Gets the cached assemblies that has been set by the <c>SetAssemblies</c> method.
    /// </summary>
    public static IEnumerable<Assembly> Assemblies
    {
      get
      {
        return ExtensionManager.assemblies;
      }
    }

    /// <summary>
    /// Gets the cached instances of the <see cref="IExtension">IExtension</see> interface.
    /// </summary>
    public static IEnumerable<IExtension> Extensions
    {
      get
      {
        if (ExtensionManager.extensions == null)
          ExtensionManager.extensions = ExtensionManager.GetInstances<IExtension>();

        return ExtensionManager.extensions;
      }
    }

    /// <summary>
    /// Sets the discovered assemblies and invalidates the <see cref="IExtension">IExtension</see> interface
    /// instances cache.
    /// </summary>
    /// <param name="assemblies">The assemblies to set.</param>
    public static void SetAssemblies(IEnumerable<Assembly> assemblies)
    {
      ExtensionManager.assemblies = assemblies;
      ExtensionManager.extensions = null;
    }

    /// <summary>
    /// Gets the first implementation of the type specified by the type parameter or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <returns></returns>
    public static Type GetImplementation<T>()
    {
      return ExtensionManager.GetImplementation<T>(null);
    }

    /// <summary>
    /// Gets the first implementation of the type specified by the type parameter and located in the assemblies
    /// filtered by the predicate or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <returns></returns>
    public static Type GetImplementation<T>(Func<Assembly, bool> predicate)
    {
      return ExtensionManager.GetImplementations<T>(predicate).FirstOrDefault();
    }

    /// <summary>
    /// Gets the implementations of the type specified by the type parameter.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <returns></returns>
    public static IEnumerable<Type> GetImplementations<T>()
    {
      return ExtensionManager.GetImplementations<T>(null);
    }

    /// <summary>
    /// Gets the implementations of the type specified by the type parameter and located in the assemblies
    /// filtered by the predicate.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <returns></returns>
    public static IEnumerable<Type> GetImplementations<T>(Func<Assembly, bool> predicate)
    {
      List<Type> implementations = new List<Type>();

      foreach (Assembly assembly in ExtensionManager.GetAssemblies(predicate))
        foreach (Type type in assembly.GetTypes())
          if (typeof(T).GetTypeInfo().IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
            implementations.Add(type);

      return implementations;
    }

    /// <summary>
    /// Gets the new instance of the first implementation of the type specified by the type parameter
    /// or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <returns></returns>
    public static T GetInstance<T>()
    {
      return ExtensionManager.GetInstance<T>(null);
    }


    /// <summary>
    /// Gets the new instance of the first implementation of the type specified by the type parameter
    /// and located in the assemblies filtered by the predicate or null if no implementations found.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementation of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <returns></returns>
    public static T GetInstance<T>(Func<Assembly, bool> predicate)
    {
      return ExtensionManager.GetInstances<T>(predicate).FirstOrDefault();
    }

    /// <summary>
    /// Gets the new instances of the implementations of the type specified by the type parameter.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <returns></returns>
    public static IEnumerable<T> GetInstances<T>()
    {
      return ExtensionManager.GetInstances<T>(null);
    }

    /// <summary>
    /// Gets the new instances of the implementations of the type specified by the type parameter
    /// and located in the assemblies filtered by the predicate.
    /// </summary>
    /// <typeparam name="T">The type parameter to find implementations of.</typeparam>
    /// <param name="predicate">The predicate to filter the assemblies.</param>
    /// <returns></returns>
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