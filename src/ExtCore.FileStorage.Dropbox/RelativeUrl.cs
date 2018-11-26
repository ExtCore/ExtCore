// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace ExtCore.FileStorage.Dropbox
{
  public class RelativeUrl
  {
    public static string Combine(params string[] segments)
    {
      StringBuilder result = new StringBuilder();

      foreach (string segment in segments)
        if (!string.IsNullOrEmpty(segment))
          result.Append((segment.StartsWith("/") ? null : "/") + segment.TrimEnd('/'));

      return result.ToString();
    }
  }
}