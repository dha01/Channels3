﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Network.Node.Service;

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
						Console.WriteLine("Запущен координационный сервер.");
						break;
					case "RunInvoke":
						RunInvokeServer();
						Console.WriteLine("Запущен вычислительный сервер.");
						break;
					case "Sum":
						Console.WriteLine(Sum(int.Parse(command[1]), int.Parse(command[2])));
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