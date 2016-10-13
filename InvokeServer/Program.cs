using System;
using Client;
using Core.Model.Network.Node.Service;

namespace InvokeServer
{
	class Program
	{
		private static InvokeNodeService _invokeNodeService;
		
		static void Main(string[] args)
		{
			_invokeNodeService = new InvokeNodeService();
			Console.WriteLine("Вычислительный сервер");
			Console.ReadKey();
		}
	}
}
