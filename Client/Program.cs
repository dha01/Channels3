using System;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Network.Node.Service;

namespace Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var node_client = new ClientNodeService(12354);

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
				node_client.Invoke(data_invoke_a);
				var data_invoke_b = new DataInvoke(2);
				node_client.Invoke(data_invoke_b);

				var data_invoke_result = new DataInvoke()
				{
					Method = method,
					InputIds = new[] { data_invoke_a.Id, data_invoke_b.Id },
					InvokeType = InvokeType.Local
				};

				node_client.Invoke(data_invoke_result);

				var result = node_client.Get(data_invoke_result.Id);

				Console.WriteLine(result);

				Console.WriteLine("End");
				Console.ReadKey();
			}
		}
	}
}
