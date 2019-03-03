// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using ExtCore.Infrastructure;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExtCore.ConsoleApplication.Extensions
{
	/// <summary>
	/// Contains the extension methods of the <see cref="IHostBuilder">IApplicationBuilder</see> interface.
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// Executes the Configure actions from all the extensions. It must be called inside the Configure method
		/// of the web application's Startup class in order ExtCore to work properly.
		/// </summary>
		/// <param name="hostBuilder">The host builder passed to the Configure method of the console application's Startup class.</param>
		/// <param name="services">The services.</param>
		public static void UseExtCore(this IHostBuilder hostBuilder, IServiceCollection services)
		{
			IServiceProvider serviceProvider = services.BuildServiceProvider();
			ILogger logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger("ExtCore.ConsoleApplication");

			foreach (IConfigureHostAction action in ExtensionManager.GetInstances<IConfigureHostAction>().OrderBy(a => a.Priority))
			{
				logger.LogInformation("Executing Configure action '{0}'", action.GetType().FullName);
				action.Execute(hostBuilder, serviceProvider);
			}
		}
	}
}