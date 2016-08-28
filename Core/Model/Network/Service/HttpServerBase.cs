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
using Core.Model.Methods.CSharp.DomainModel;
using Newtonsoft.Json;

namespace Core.Model.Network.Service
{
	public class WebAction
	{
		public Type InputType { get; set; }
		public MethodInfo MethodInfo { get; set; }
		public object Object { get; set; }

		public object Invoke(object[] inputs)
		{
			return MethodInfo.Invoke(Object, inputs);
		}
	}
	
	public class HttpServerBase
	{
		private const int DEFAULT_PORT = 1234;
		
		public int Port = 80;

		public Dictionary<string, Dictionary<string, WebAction>> UrlPaths;
		private HttpListener Listener; // Объект, принимающий TCP-клиентов

		public HttpServerBase()
			: this(DEFAULT_PORT)
		{
			
		}

		public HttpServerBase(int port)
		{
			UrlPaths = new Dictionary<string, Dictionary<string, WebAction>>();
			
			Port = port;

			// Создаем "слушателя" для указанного порта
			Listener = new HttpListener();// TcpListener(IPAddress.Any, Port);

			Listener.Prefixes.Add(string.Format("http://127.0.0.1:{0}/", Port));
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

						var url_path = context.Request.RawUrl.Split('/');

						object result = null;

						if (UrlPaths.ContainsKey(url_path[1]))
						{
							var lvl1 = UrlPaths[url_path[1]];
							
							if (UrlPaths[url_path[1]].ContainsKey(url_path[2]))
							{
								var lvl2 = lvl1[url_path[2]];

								object input = typeof(void);
								if (lvl2.InputType != null)
								{
									input = JsonConvert.DeserializeObject(content, lvl2.InputType);
								}
								result = lvl2.Invoke(new []{input});

							}
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
	}
}
