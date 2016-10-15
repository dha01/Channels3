using System;
using System.Linq;
using Core.Model.Data.DataModel;
using Core.Model.InvokeMethods.Local.CSharp.Methods.DataModel;
using Core.Model.InvokeMethods.Local.ExecutableFile.Methods.DataModel;
using Core.Model.Network.Node.Service;
using Core.Model.Network.Service;

namespace ClientServer
{
	class Program
	{
		private static InvokeNodeService _invokeNodeService;
		private static CoordinationNodeService _coordinationNodeService;
		
		static void Main(string[] args)
		{
			
			
			Console.WriteLine("OS : {0}", Environment.OSVersion);
			Console.WriteLine("pid {0} ip {1}", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp());
			
			try
			{
				Console.Write("Input command: ");
				while (!ExecCommand(Console.ReadLine().Split(' ')))
				{
				}
				Console.WriteLine("");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

		}

		static bool ExecCommand(string[] command)
		{
			try
			{
				switch (command.First())
				{
					case "end":
						return true;
					case "pid":
						if (Environment.GetEnvironmentVariables()["SLURM_PROCID"].Equals(command[1]))
						{
							return ExecCommand(command.Skip(2).ToArray());
						}
						break;
					case "RunClient":
						ClientNodeExtension.Init();
						Console.WriteLine("pid {0} ip {1} Запущено клиентское приложение.", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp());
						break;
					case "RunCoordination":
						RunCoordinationServer();
						Console.WriteLine("pid {0} ip {1} Запущен координационный сервер.", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp());
						break;
					case "RunInvoke":
						RunInvokeServer();
						Console.WriteLine("pid {0} ip {1} Запущен вычислительный сервер.", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp());
						break;
					case "Sum":
						Console.WriteLine("Результат: {0}", Sum(int.Parse(command[1]), int.Parse(command[2])));
						break;
					case "Sum256":
						Console.WriteLine("Результат: {0}", Sum128(int.Parse(command[1]), int.Parse(command[2])));
						break;
					case "Text":
						Console.WriteLine("Результат: {0}", Text(string.Join(" ", command.Skip(1))));
						break;
					case "Run":
						var method = new ExecutableFileMethod
						{
							MethodName = command[1],
							InputParamsTypeNames = new[] { typeof(string).FullName }
						};
						var arguments = string.Join(" ", command.Skip(2));
						var result = method.Invoke(arguments);
						Console.WriteLine("Результат: {0}", result.Result());
						break;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Ошибка при выполнении команды {0}: {1}", string.Join(" ", command), e.Message);
			}
			return false;
		}

		static void RunCoordinationServer()
		{
			_coordinationNodeService = new CoordinationNodeService();
		}

		static void RunInvokeServer()
		{
			_invokeNodeService = new InvokeNodeService();
		}

		static string Text(string text)
		{
			var method = new ExecutableFileMethod
			{
				Namespace = "",
				Version = "1.0.0.0",
				TypeName = "",
				MethodName = "Text.exe",
				InputParamsTypeNames = new [] { typeof(string).FullName }
			};

			return method.Invoke<string>(text).Result();
		}

		static int Sum(int a, int b)
		{
			var method = new CSharpMethod()
			{
				Namespace = "Core",
				Version = "1.0.0.0",
				TypeName = "Core.Model.BasicMethods.Math.Service.Simple",
				MethodName = "Sum",
				InputParamsTypeNames = new[] { typeof(int).FullName, typeof(int).FullName }
			};

			return method.Invoke<int>(a, b).Result();
		}

		static CSharpMethod MethodSum = new CSharpMethod()
		{
			Namespace = "Core",
			Version = "1.0.0.0",
			TypeName = "Core.Model.BasicMethods.Math.Service.Simple",
			MethodName = "Sum",
			InputParamsTypeNames = new[] { typeof(int).FullName, typeof(int).FullName }
		};

		static DataInvoke<int>[] Sum(DataInvoke<int>[] array)
		{
			var half_size = array.Length/2;
			var arr = new DataInvoke<int>[half_size];
			
			for (int i = 0; i < half_size; i++)
			{
				var a = array[i];
				var b = array[i + half_size];
				
				arr[i] = MethodSum.Invoke<int>(a, b);
			}

			return arr;
		}

		static int Sum128(int a, int b)
		{
			var method = new CSharpMethod()
			{
				Namespace = "Core",
				Version = "1.0.0.0",
				TypeName = "Core.Model.BasicMethods.Math.Service.Simple",
				MethodName = "Sum",
				InputParamsTypeNames = new[] { typeof(int).FullName, typeof(int).FullName }
			};

			DataInvoke<int>[] arr = new DataInvoke<int>[128];
			for (int i = 0; i < 128; i++)
			{
				arr[i] = method.Invoke<int>(a, b);
			}

			while (arr.Length > 1)
			{
				arr = Sum(arr);
			}

			return arr[0].Result();
		}
	}
}
