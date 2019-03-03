using System;
using System.Collections.Generic;
using System.Text;
using ExtCore.Infrastructure.Actions;
using ExtensionA.Interfaces;
using ExtensionA.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExtensionA.Actions
{
	public class ConfigureAction : IConfigureHostAction
	{
		public int Priority => 1000;

		public void Execute(IHostBuilder hostBuilder, IServiceCollection services)
		{
			services.AddScoped<ITestService, TestService>();
		}
	}
}
