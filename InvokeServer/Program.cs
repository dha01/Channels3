using System;
using Client;
using Core.Model.Network.DataModel;
using Core.Model.Server.Service;

namespace InvokeServer
{
	class Program
	{
		private static InvokeServerService _invokeServerService;
		
		static void Main(string[] args)
		{
			var remote_server_node = new Node()
			{
				URL = "127.0.0.1",
				Port = 12354
			};

			var local_server_node = new Node()
			{
				URL = "127.0.0.1",
				Port = 12345
			};

			_invokeServerService = new InvokeServerService(string.Format("http://127.0.0.1:{0}/", local_server_node.Port));

			// TODO: Нужно добавить config с папками из которых будут загружаться уже имеющиеся библиотеки.
			_invokeServerService._cSharpAssemblyService.AddAssembly(typeof(SomeClass).Assembly.Location);

			// TODO: Нужно добавить поиск по UDP серверов в текущей сети.
			// TODO: Нужно добавить config со списком доступных серверов.
			_invokeServerService._coordinationService.AddNode(remote_server_node);

			Console.WriteLine("Сервер");
			Console.ReadKey();
		}
	}
}
