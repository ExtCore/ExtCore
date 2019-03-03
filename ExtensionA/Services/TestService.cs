using ExtensionA.Interfaces;

namespace ExtensionA.Services
{
	public class TestService : ITestService
	{
		public string Hello()
		{
			return "Hello";
		}
	}
}
