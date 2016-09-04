using System;
using System.Collections.Generic;
using System.Linq;
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
		
		protected WebServerServiceBase()
		{
			_routes = new Dictionary<string, MethodInfo>();

			var type = GetType();
			var web_methods = type.GetMethods().Where(x => x.GetCustomAttributes(typeof(WebMethodAttribute)).Any()).ToList();
			InitWebMethods(web_methods);
		}

		private void InitWebMethods(IEnumerable<MethodInfo> method_infos)
		{
			var type = GetType();
			var name = ((WebClass)type.GetCustomAttributes(typeof(WebClass)).First()).Namespace;
			foreach (var web_method in method_infos)
			{
				AddWebMethod(name, web_method);
			}
		}

		public abstract void AddWebMethod(string root, MethodInfo method);
	}
}
