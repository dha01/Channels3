using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Network.DataModel;

namespace Core.Model.Network.Service
{
	
	[WebClass(Namespace = "Base")]
	public abstract class WebServerServiceBase : IWebServerService
	{
		protected Dictionary<string, MethodInfo> _routes;

		protected Dictionary<MethodInfo, object> _invokedObjects;
		
		protected WebServerServiceBase()
		{
			_routes = new Dictionary<string, MethodInfo>();
			_invokedObjects = new Dictionary<MethodInfo, object>();
			InitWebMethods(this);
		}

		public void InitWebMethods(object invoked_object)
		{
			var type = invoked_object.GetType();
			var name = ((WebClass)type.GetCustomAttributes(typeof(WebClass)).First()).Namespace;
			var method_infos = type.GetMethods().Where(x => x.GetCustomAttributes(typeof(WebMethodAttribute)).Any()).ToList();

			foreach (var web_method in method_infos)
			{
				AddWebMethod(name, web_method);
				_invokedObjects.Add(web_method, invoked_object);
			}
		}

		protected virtual object InvokeWebMethod(string route, object[] input_params)
		{
			if (!_routes.ContainsKey(route))
			{
				throw new Exception(string.Format("{0}->Метод не найден.", GetType().Name));
			}
			var method_info = _routes[route];
			var invoked_object = _invokedObjects[method_info];
			return method_info.Invoke(invoked_object, input_params);
		}

		public abstract void AddWebMethod(string root, MethodInfo method);

		public abstract T Request<T>(NodeInfo node_info, string name, object input_param);

		public abstract void Request(NodeInfo node_info, string name, object input_param);

		public static string GetLocalIp()
		{
			return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
		}
	}
}
