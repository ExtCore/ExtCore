// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ExtCore.Infrastructure
{
  /// <summary>
  /// Describes an extension.
  /// </summary>
  public interface IExtension
  {
    /// <summary>
    /// Gets the name of the extension.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the description of the extension.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the URL of the extension.
    /// </summary>
    string Url { get; }

    /// <summary>
    /// Gets the version of the extension.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Gets the authors of the extension (separated by commas).
    /// </summary>
    string Authors { get; }
  }
}