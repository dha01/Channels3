using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Network.Node.Service;

namespace CoordinationServer
{
	class Program
	{
		private static CoordinationNodeService _coordinationNodeService;
		
		static void Main(string[] args)
		{
			_coordinationNodeService = new CoordinationNodeService();
			Console.WriteLine("Координационный сервер");
			Console.ReadKey();
		}
	}
}
