using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Client;
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

namespace InvokeServer
{
	class Program
	{
		private static IInvokeServerService _invokeServerService;
		private static ISendRequestService _sendRequestService;
		//private static IReceiveRequestService<Request> _receiveRequestService;
		private static IDataService<DataInvoke> _dataService;
		private static IDataCollectorService _dataCollectorService;
		//private static IAssemblyService _assemblyService;
		
		static void Main(string[] args)
		{
			var v = typeof(SomeClass).GetMethods();
			
			_sendRequestService = new SendRequestService();
			//_receiveRequestService = null; //new ReceiveRequestService<Request>();
			_dataService = new DataService<DataInvoke>();

			var assembly_service = new AssemblyService();
			var assembly_service_factory = new AssemblyServiceFactory(assembly_service);

			var method_service = new MethodService(assembly_service_factory);
			var coordination_service = new CoordinationService();
			coordination_service.AddNode(new Node()
			{
				IpAddress = "127.0.0.1",
				Port = 1234
			});

			var send_request_service = new SendRequestService();

			var invoke_service_factory = new InvokeServiceFactory(method_service, assembly_service, coordination_service, send_request_service, _dataService);


			_dataCollectorService = new DataCollectorService(InvokeType.Local, invoke_service_factory, _dataService, send_request_service); ;


			var assembly = typeof (SomeClass).Assembly;
			var n = assembly.GetName();
			assembly_service.AddAssembly(new AssemblyFile()
			{
				Data = File.ReadAllBytes(assembly.Location),
				Version = assembly.GetName().Version.ToString(),
				Namespace = assembly.GetName().Name
			});



			var method_base = new CSharpMethod
			{
				Version = "1.0.0.0",
				Namespace = "Core",
				TypeName = "Client.SomeClass",
				MethodName = "Sum",
				InputParamsTypeNames = new[] { typeof(double).FullName, typeof(double).FullName }
			};

			var method = method_service.GetMethod(method_base);
			method_service.AddMethod(method);

			var data = new DataInvoke(125);

			_dataService.Add(data);
			Console.WriteLine(data.Id);
			_invokeServerService = new InvokeServerService(_sendRequestService, _dataService, _dataCollectorService, assembly_service, new HttpServerBase());


			


			Console.ReadKey();
		}
	}
}
