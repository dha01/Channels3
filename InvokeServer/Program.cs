using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.DataModel;
using Core.Model.Network.Service;
using Core.Model.Server.Service;

namespace InvokeServer
{
	class Program
	{
		private static IInvokeServerService _invokeServerService;
		private static ISendRequestService _sendRequestService;
		//private static IReceiveRequestService<Request> _receiveRequestService;
		private static IDataService<DataInvoke> _dataService;
		private static IDataCollectorService _dataCollectorService;
		private static IAssemblyService _assemblyService;
		
		static void Main(string[] args)
		{
			_sendRequestService = new SendRequestService();
			//_receiveRequestService = null; //new ReceiveRequestService<Request>();
			_dataService = new DataService<DataInvoke>();


			var assembly_service = new AssemblyService();
			var method_service = new MethodService();
			var coordination_service = new CoordinationService();
			coordination_service.AddNode(new Node()
			{
				IpAddress = "127.0.0.1",
				Port = 1234
			});

			var send_request_service = new SendRequestService();

			var invoke_service_factory = new InvokeServiceFactory(method_service, assembly_service, coordination_service, send_request_service, _dataService);


			_dataCollectorService = new DataCollectorService(InvokeType.Local, invoke_service_factory, _dataService, send_request_service); ;
			_assemblyService = new AssemblyService();

			var method_id = _assemblyService.AddMethod(typeof(SomeClass).GetMethod("Sum"));
			var method = _assemblyService.GetMethods(typeof(SomeClass)).First(x => x.Id == method_id);
			method_service.AddMethod(method);

			var data = new DataInvoke(125);

			_dataService.Add(data);
			Console.WriteLine(data.Id);
			_invokeServerService = new InvokeServerService(_sendRequestService, _dataService, _dataCollectorService, _assemblyService, new HttpServerBase());


			


			Console.ReadKey();
		}
	}
}
