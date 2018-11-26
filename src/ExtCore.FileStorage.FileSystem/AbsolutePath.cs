// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;

namespace ExtCore.FileStorage.FileSystem
{
  public static class AbsolutePath
  {
    public static string Combine(params string[] segments)
    {
      return string.Join(
        Path.DirectorySeparatorChar.ToString(),
        segments.Where(s => !string.IsNullOrEmpty(s)).Select(
          s => string.Join(Path.DirectorySeparatorChar.ToString(), s.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries))
        ).Where(s => !string.IsNullOrEmpty(s))
      );
    }
  }
}