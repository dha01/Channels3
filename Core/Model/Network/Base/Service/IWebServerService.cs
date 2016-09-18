using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Network.DataModel;

namespace Core.Model.Network.Service
{
	public interface IWebServerService
	{
		int Port { get; }

		void AddWebMethod(string uri, MethodInfo method);

		void InitWebMethods(object invoked_object);

		T Request<T>(NodeInfo node_info, string name, object input_param);

		void Request(NodeInfo node_info, string name, object input_param);
	}
}
