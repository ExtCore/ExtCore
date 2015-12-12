// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNet.Mvc;

namespace ExtCore.WebApplication
{
  public static class ControllerExtensions
  {
    public static ActionResult Stream(this Controller controller, Stream stream)
    {
      return new StreamResult(stream);
    }
  }
}