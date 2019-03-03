using System;
using System.IO;
using System.Threading.Tasks;
using ExtCore.ConsoleApplication.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace TestConsoleApp
{
	public class Program
	{
		private static string _extensionsPath;

		public static async Task Main(string[] args)
		{
			var host = new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config.AddJsonFile("appsettings.json", optional: true);
					config.AddEnvironmentVariables();
					if (args != null)
					{
						config.AddCommandLine(args);
					}
					_extensionsPath = Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "Extensions");
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddConsole();
				})
				.ConfigureServices((hostingContext, services) =>
				{
					services.AddOptions();
					services.Configure<HostOptions>(option => { option.ShutdownTimeout = System.TimeSpan.FromSeconds(20); });
					services.AddExtCore(_extensionsPath);
				})
				.UseExtCore()
				.Build();
			await host.RunAsync();
		}
	}
}
