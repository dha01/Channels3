using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Network.Node.Service;
using Core.Model.Server.Service;

namespace CoordinationServer
{
	class Program
	{
		private static INodeService _coordinationServerService;
		
		static void Main(string[] args)
		{
			_coordinationServerService = new CoordinationNodeService(12347);

			Console.WriteLine("Координационный сервер");
			Console.ReadKey();
		}
	}
}
