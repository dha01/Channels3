using System;
using System.Linq;
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
			ClientNodeExtension.Init();
			
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
					case "RunCoordination":
						RunCoordinationServer();
						Console.WriteLine("pid {0} ip {1} Запущен координационный сервер.", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp());
						break;
					case "RunInvoke":
						RunInvokeServer();
						Console.WriteLine("pid {0} ip {1} Запущен вычислительный сервер.", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp());
						break;
					case "Sum":
						Console.WriteLine(Sum(int.Parse(command[1]), int.Parse(command[2])));
						break;
					case "Text":
						Console.WriteLine(Text());
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

		static string Text()
		{
			var method = new ExecutableFileMethod
			{
				Namespace = "",
				Version = "1.0.0.0",
				TypeName = "",
				MethodName = "Text.exe",
				InputParamsTypeNames = new string[] { }
			};

			return method.Invoke<string>().Result();
		}

		static int Sum(int a, int b)
		{
			var method = new CSharpMethod()
			{
				Namespace = "Core",
				Version = "1.0.0.0",
				TypeName = "Core.Model.BasicMethods.Math.Service.Simple",
				MethodName = "Sum",
				InputParamsTypeNames = new[] { "System.Int32", "System.Int32" }
			};

			return method.Invoke<int>(a, b).Result();
		}
	}
}
