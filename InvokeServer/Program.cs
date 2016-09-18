using System;
using Client;
using Core.Model.Network.DataModel;
using Core.Model.Network.Node.Service;
using Core.Model.Server.Service;

namespace InvokeServer
{
	class Program
	{
		private static InvokeNodeService _invokeNodeService;
		
		static void Main(string[] args)
		{
			_invokeNodeService = new InvokeNodeService(12345);
			Console.WriteLine("Вычислительный сервер");
			Console.ReadKey();
		}
	}
}
