using ExtCore.Infrastructure.Actions;
using HostedServiceA.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostedServiceA.Actions
{
	public class AddHostedServiceAction : IConfigureHostAction
	{
		public int Priority => 1000;
		
		public void Execute(IHostBuilder hostBuilder, IServiceCollection services)
		{
			services.AddHostedService<TimedHostedServiceA>();
		}
	}
}
