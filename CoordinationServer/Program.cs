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
		private static CoordinationNodeService _coordinationNodeService;
		
		static void Main(string[] args)
		{
			_coordinationNodeService = new CoordinationNodeService(12347);
			Console.WriteLine("Координационный сервер");
			Console.ReadKey();
		}
	}
}
