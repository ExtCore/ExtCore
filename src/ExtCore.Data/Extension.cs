// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Infrastructure;

namespace ExtCore.Data
{
  /// <summary>
  /// Overrides the <see cref="ExtensionBase">ExtensionBase</see> class and provides the ExtCore.Data extension information.
  /// </summary>
  public class Extension : ExtensionBase
  {
    /// <summary>
    /// Gets the name of the extension.
    /// </summary>
    public override string Name => "ExtCore.Data";

    /// <summary>
    /// Gets the URL of the extension.
    /// </summary>
    public override string Url => "http://extcore.net/";

    /// <summary>
    /// Gets the version of the extension.
    /// </summary>
    public override string Version => "5.0.0";

    /// <summary>
    /// Gets the authors of the extension (separated by commas).
    /// </summary>
    public override string Authors => "Dmitry Sikorsky";
  }
}