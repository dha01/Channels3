using System;
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
		static void Main(string[] args)
		{
			var v = typeof (SomeClass).GetMethods().First().GetParameters().First().ParameterType;
			
			var assembly_service = new AssemblyService();
			var assembly_service_factory = new AssemblyServiceFactory(assembly_service);

			var method_service = new MethodService(assembly_service_factory);
			var coordination_service = new CoordinationService();
			coordination_service.AddNode(new Node()
			{
				IpAddress = "127.0.0.1",
				Port = 1234
			});

			assembly_service.AddAssembly(typeof(SomeClass).Assembly);

			var send_request_service = new SendRequestService();

			var data_service = new DataService<DataInvoke>();
			var invoke_service_factory = new InvokeServiceFactory(method_service, assembly_service, coordination_service, send_request_service, data_service);


			var data_collector = new DataCollectorService(InvokeType.Remote, invoke_service_factory, data_service, send_request_service);

			var method = new CSharpMethod()
			{
				Version = "1.0.0.0",
				Namespace = "Core",
				TypeName = "Client.SomeClass",
				MethodName = "Sum",
				InputParamsTypeNames = new[] { "System.Double", "System.Double" }
			};

			var _invokeServerService = new InvokeServerService(send_request_service, data_service, data_collector, assembly_service, new HttpServerBase(1235));
			Console.ReadKey();
			int i = 0;
			while (true)
			{
				i++;
				var data_invoke_a = new DataInvoke(i);
				data_collector.Invoke(data_invoke_a);
				var data_invoke_b = new DataInvoke(2);
				data_collector.Invoke(data_invoke_b);

				var data_invoke_result = new DataInvoke()
				{
					Method = method,
					InputIds = new[] { data_invoke_a.Id, data_invoke_b.Id },
					InvokeType = InvokeType.Local,
					Sender = new Node()
					{
						IpAddress = "127.0.0.1",
						Port = 1235
					}
				};

				data_collector.Invoke(data_invoke_result);

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

				var result = data_collector.Get(data_invoke_result.Id);

				Console.WriteLine(result);

				Console.WriteLine("End");
				Console.ReadKey();
			}
		}
	}
}
