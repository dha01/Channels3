using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Network.Service
{
	public interface IWebServerService
	{
		void AddWebMethod(string uri, MethodInfo method);
	}
}
