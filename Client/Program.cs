﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.DataModel;
using Core.Model.Network.Service;
using Core.Model.Server.Service;

namespace Client
{
	class Program
	{
		private static InvokeServerService _invokeServerService;
		private static ISendRequestService _sendRequestService;
		private static IDataService<DataInvoke> _dataService;
		private static IDataCollectorService _dataCollectorService;
		private static ICSharpAssemblyService _cSharpAssemblyService;
		private static IAssemblyServiceFactory _assemblyServiceFactory;
		private static IMethodService _methodService;
		private static ICoordinationService _coordinationService;
		private static IInvokeServiceFactory _invokeServiceFactory;

		private static HttpServerBase _httpServerBase;
		
		static void Main(string[] args)
		{
			var remote_server_node = new Node()
			{
				URL = "127.0.0.1",
				Port = 12345
			};

			var local_server_node = new Node()
			{
				URL = "127.0.0.1",
				Port = 12354
			};

			_cSharpAssemblyService = new CSharpAssemblyService();
			_assemblyServiceFactory = new AssemblyServiceFactory(_cSharpAssemblyService);
			_methodService = new MethodService(_assemblyServiceFactory);
			_coordinationService = new CoordinationService();
			_sendRequestService = new SendRequestService();
			_dataService = new DataService<DataInvoke>();
			_invokeServiceFactory = new InvokeServiceFactory(_methodService, _cSharpAssemblyService, _coordinationService, _sendRequestService, _dataService);
			_dataCollectorService = new DataCollectorService(InvokeType.Remote, _invokeServiceFactory, _dataService, _sendRequestService);
			_invokeServerService = new InvokeServerService(string.Format("http://127.0.0.1:{0}/", local_server_node.Port), _dataService, _dataCollectorService, _cSharpAssemblyService);

			_cSharpAssemblyService.AddAssembly(typeof(SomeClass).Assembly);
			_coordinationService.AddNode(remote_server_node);

			var method = new CSharpMethod()
			{
				Version = "1.0.0.0",
				Namespace = "Core",
				TypeName = "Client.SomeClass",
				MethodName = "Sum",
				InputParamsTypeNames = new[] { "System.Double", "System.Double" }
			};

			Console.WriteLine("Клиент");
			Console.ReadKey();
			int i = 0;
			while (true)
			{
				i++;
				var data_invoke_a = new DataInvoke(i);
				_dataCollectorService.Invoke(data_invoke_a);
				var data_invoke_b = new DataInvoke(2);
				_dataCollectorService.Invoke(data_invoke_b);

				var data_invoke_result = new DataInvoke()
				{
					Method = method,
					InputIds = new[] { data_invoke_a.Id, data_invoke_b.Id },
					InvokeType = InvokeType.Local,
					Sender = local_server_node
				};

				_dataCollectorService.Invoke(data_invoke_result);

				/*
				for (int j = 0; j < 1000; j++)
				{
					var data_invoke_result2 = new DataInvoke()
					{
						Method = method,
						InputIds = new[] { data_invoke_a.Id, data_invoke_result.Id },
						InvokeType = InvokeType.Local
					};
					data_collector.Invoke(data_invoke_result2);

					data_invoke_result = data_invoke_result2;
				}*/

				var result = _dataCollectorService.Get(data_invoke_result.Id);

				Console.WriteLine(result);

				Console.WriteLine("End");
				Console.ReadKey();
			}
		}
	}
}
