using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.Service;
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
			
			var assembly_service = new AssemblyService();
			var method_service = new MethodService();
			var coordination_service = new CoordinationService();
			coordination_service.AddNode(new Node()
			{
				IpAddress = "127.0.0.1",
				Port = 1234
			});

			var send_request_service = new SendRequestService();

			var data_service = new DataService<DataInvoke>();
			var invoke_service_factory = new InvokeServiceFactory(method_service, assembly_service, coordination_service, send_request_service, data_service);


			var data_collector = new DataCollectorService(InvokeType.Remote, invoke_service_factory, data_service, send_request_service);

			var method_id = assembly_service.AddMethod(typeof (SomeClass).GetMethod("Sum"));
			var method = assembly_service.GetMethods(typeof (SomeClass)).First(x => x.Id == method_id);
			method_service.AddMethod(method);

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
					MethodId = method_id,
					InputIds = new[] { data_invoke_a.Id, data_invoke_b.Id },
					InvokeType = InvokeType.Remote,
					Sender = new Node()
					{
						IpAddress = "127.0.0.1",
						Port = 1235
					}
				};

				data_collector.Invoke(data_invoke_result);

				
				for (int j = 0; j < 1000; j++)
				{
					var data_invoke_result2 = new DataInvoke()
					{
						MethodId = method_id,
						InputIds = new[] { data_invoke_a.Id, data_invoke_result.Id },
						InvokeType = InvokeType.Local
					};
					data_collector.Invoke(data_invoke_result2);

					data_invoke_result = data_invoke_result2;
				}

				var result = data_collector.Get(data_invoke_result.Id);

				Console.WriteLine(result);

				Console.WriteLine("End");
				Console.ReadKey();
			}
		}
	}
}
