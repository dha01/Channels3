using System;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Network.Node.Service;

namespace Client
{
	class Program
	{
		static void Main(string[] args)
		{
			ClientNodeExtension.Init();

			var method = new CSharpMethod()
			{
				Namespace = "Core",
				Version = "1.0.0.0",
				TypeName = "Core.Model.BasicMethods.Math.Service.Simple",
				MethodName = "Sum",
				InputParamsTypeNames = new[] { "System.Int32", "System.Int32" }
			};

			Console.WriteLine("Клиент");
			Console.ReadKey();
			int i = 0;
			while (true)
			{
				i++;
				var a = method.Invoke<int>(1, 2);
				var b = method.Invoke<int>(3, 4);
				var c = method.Invoke<int>(5, 6);
				var d = method.Invoke<int>(7, 8);

				var e = method.Invoke<int>(a, b);
				var f = method.Invoke<int>(c, d);

				var result = method.Invoke<int>(e, f);
				var r = result.Result();
				Console.WriteLine(r);

				Console.WriteLine("End");
				Console.ReadKey();
			}
		}
	}
}
