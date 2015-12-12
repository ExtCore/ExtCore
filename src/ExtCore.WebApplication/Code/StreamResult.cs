// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;

namespace ExtCore.WebApplication
{
  public class StreamResult : ActionResult
  {
    private Stream stream;

    public StreamResult(Stream stream)
    {
      this.stream = stream;
    }

    public async override Task ExecuteResultAsync(ActionContext actionContext)
    {
      HttpResponse httpResponse = actionContext.HttpContext.Response;

      await this.stream.CopyToAsync(httpResponse.Body);
    }
  }
}