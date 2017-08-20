// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reflection;

namespace ExtCore.WebApplication
{
  /// <summary>
  /// Describes an assembly provider with the mechanism of getting assemblies that should be involved
  /// in the ExtCore types discovering process.
  /// </summary>
  public interface IAssemblyProvider
  {
    /// <summary>
    /// Discovers and then gets the discovered assemblies.
    /// </summary>
    /// <param name="path">The extensions path of a web application. Might be used or ignored
    /// by an implementation of the <see cref="IAssemblyProvider">IAssemblyProvider</see> interface.</param>
    /// <param name="includingSubpaths">
    /// Determines whether a web application will discover and then get the discovered assemblies from the subfolders
    /// of a specific folder recursively. Might be used or ignored by an implementation of the
    /// <see cref="IAssemblyProvider">IAssemblyProvider</see> interface.</param>
    /// </param>
    /// <returns>The discovered and loaded assemblies.</returns>
    IEnumerable<Assembly> GetAssemblies(string path, bool includingSubpaths);
  }
}