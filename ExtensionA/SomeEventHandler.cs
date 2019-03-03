using System;
using TestConsoleApp.Events;

namespace ExtensionA
{
	public class SomeEventHandler : ISomeActionEventHandler
	{
		public int Priority => 1000;

		public void HandleEvent(string argument)
		{
			// Using Console because I can't inject services to get logging
			Console.WriteLine(argument);
		}
	}
}
