using System;
using Client;
using Core.Model.Network.DataModel;
using Core.Model.Network.Node.Service;
using Core.Model.Server.Service;

namespace InvokeServer
{
	class Program
	{
		private static InvokeServerService _invokeServerService;
		
		static void Main(string[] args)
		{
			InvokeNodeService invoke_node_service = new InvokeNodeService(12345);
			
			//_invokeServerService = new InvokeServerService(12345);

			// TODO: Нужно добавить поиск по UDP серверов в текущей сети.
			// TODO: Нужно добавить config со списком доступных серверов.
			//_invokeServerService._coordinationService.AddNode(remote_server_node);

			Console.WriteLine("Вычислительный сервер");
			Console.ReadKey();
		}
	}
}
