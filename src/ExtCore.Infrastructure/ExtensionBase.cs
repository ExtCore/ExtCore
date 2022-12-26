// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ExtCore.Infrastructure;

/// <summary>
/// Implements the <see cref="IExtension">IExtension</see> interface and represents default extension behavior.
/// </summary>
public abstract class ExtensionBase : IExtension
{
  /// <summary>
  /// Gets the name of the extension.
  /// </summary>
  public virtual string Name => this.GetType().FullName;

  /// <summary>
  /// Gets the description of the extension.
  /// </summary>
  public virtual string Description => null;

  /// <summary>
  /// Gets the URL of the extension.
  /// </summary>
  public virtual string Url => null;

  /// <summary>
  /// Gets the version of the extension.
  /// </summary>
  public virtual string Version => null;

  /// <summary>
  /// Gets the authors of the extension (separated by commas).
  /// </summary>
  public virtual string Authors => null;
}