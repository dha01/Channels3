using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Network.DataModel;
using Newtonsoft.Json;

namespace Core.Model.Network.Service
{
	[WebClass(Namespace = "Default")]
	public class HttpServerService : WebServerServiceBase
	{
		private const string DEFAULT_URL = "http://127.0.0.1:1234/";

		public string Url;

		private HttpListener Listener; // Объект, принимающий TCP-клиентов

		public HttpServerService()
			: this(DEFAULT_URL)
		{
			
		}
		
		public HttpServerService(string url)
			: base()
		{
			Url = url;

			// Создаем "слушателя" для указанного порта
			Listener = new HttpListener();// TcpListener(IPAddress.Any, Port);

			Listener.Prefixes.Add(Url);
			Listener.Start(); // Запускаем его

			Task.Run(() =>
			{
				// В бесконечном цикле
				while (true)
				{
					try
					{
						HttpListenerContext context = Listener.GetContext();
						HttpListenerResponse response = context.Response;

						string page = Directory.GetCurrentDirectory() + context.Request.Url.LocalPath;
						var sr = new StreamReader(context.Request.InputStream);
						string content = sr.ReadToEnd();
						var n = new DataInvoke();
						n.InvokeType = InvokeType.Local;
						n.Value = new KeyValuePair<int, int>(1,2);

						var url_path = context.Request.RawUrl;

						object result = null;

						if (_routes.ContainsKey(url_path))
						{
							var method_info = _routes[url_path];

							var input_parameters = method_info.GetParameters();
							object input = typeof(void);


							if (input_parameters.Any())
							{
								input = JsonConvert.DeserializeObject(content, input_parameters.First().ParameterType);
							}
							result = method_info.Invoke(this, new[] { input });
						}

						var json_result = JsonConvert.SerializeObject(result);

						byte[] buffer = Encoding.UTF8.GetBytes(json_result);

						response.ContentLength64 = buffer.Length;
						Stream st = response.OutputStream;
						st.Write(buffer, 0, buffer.Length);

						context.Response.Close();
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
				}
			});
		}

		public override void AddWebMethod(string root, MethodInfo method)
		{
			_routes.Add(string.Format(@"/{0}/{1}", root, method.Name), method);
		}
	}
}
