// Copyright © 2022 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;

namespace ExtCore.FileStorage.Azure;

public static class RelativeUrl
{
  public static string Combine(params string[] segments)
  {
    return string.Join(
      "/",
      segments.Where(s => !string.IsNullOrEmpty(s)).Select(
        s => string.Join("/", s.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries))
      ).Where(s => !string.IsNullOrEmpty(s))
    );
  }
}