﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
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

		private const int DEFAULT_PORT = 1234;

		//public string Url;

		private HttpListener Listener; // Объект, принимающий TCP-клиентов

		#region Constructor
		
		public HttpServerService()
			: this(DEFAULT_PORT)
		{
			
		}
		
		public HttpServerService(int port)
			: base()
		{
			_port = port;
			// Создаем "слушателя" для указанного порта
			Listener = new HttpListener();
			
			string host = Dns.GetHostName();
			var ips = Dns.GetHostEntry(host).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList();

			foreach (var ip in ips)
			{
				AddAddress(string.Format("http://{0}:{1}/", ip, port));
			}
			//Listener.Prefixes.Add(Url);
			Listener.Start(); // Запускаем его

			Task.Run(() =>
			{
				// В бесконечном цикле
				while (true)
				{
					try
					{
						var context = Listener.GetContext();

						// Вызываем метод.
						var result = InvokeWebMethod(context.Request.RawUrl, ExtractJsonInputParams(context.Request));

						// Отправляем обратно результат.
						SendResponce(context.Response, result);

						// Закрываем соединение.
						context.Response.Close();
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
				}
			});
		}

		#endregion

		public override void AddWebMethod(string root, MethodInfo method)
		{
			_routes.Add(string.Format(@"/{0}/{1}", root, method.Name), method);
		}

		#region Methods/Private

		private void AddAddress(string url)
		{
			Listener.Prefixes.Add(url);
		}

		/// <summary>
		/// Отправляет ответ.
		/// </summary>
		/// <param name="http_listener_response"></param>
		/// <param name="value">Отправляемое значение.</param>
		private void SendResponce(HttpListenerResponse http_listener_response, object value)
		{
			var json_result = JsonConvert.SerializeObject(value);
			var buffer = Encoding.UTF8.GetBytes(json_result);

			http_listener_response.ContentLength64 += buffer.Length;
			http_listener_response.OutputStream.Write(buffer, 0, buffer.Length);
		}

		/// <summary>
		/// Извлекает и возвращает входные параметры из запроса.
		/// </summary>
		/// <param name="http_listener_request"></param>
		/// <returns></returns>
		private string ExtractJsonInputParams(HttpListenerRequest http_listener_request)
		{
			return new StreamReader(http_listener_request.InputStream).ReadToEnd();
		}

		/// <summary>
		/// Вызывает метод и возвращает результат его исполнения.
		/// </summary>
		/// <param name="url_path">URL метода.</param>
		/// <param name="json_input_params">Входные параметры в формате JSON.</param>
		/// <returns>Результат исполнения метода.</returns>
		protected object InvokeWebMethod(string url_path, string json_input_params)
		{
			if (!_routes.ContainsKey(url_path))
			{
				throw new Exception("HttpServerService->Метод не найден.");
			}

			var method_info = _routes[url_path];
			var input_parameters = method_info.GetParameters();
			object input = typeof(void);

			if (input_parameters.Any())
			{
				input = JsonConvert.DeserializeObject(json_input_params, input_parameters.First().ParameterType);
			}
			return InvokeWebMethod(url_path, new[] { input });
		}

		#endregion

		public override T Request<T>(NodeInfo node_info, string name, object input_param)
		{
			using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(input_param));
				var response = client.PostAsync(string.Format("http://{0}:{1}/{2}", node_info.URL, node_info.Port, name), content);
				var responseString = response.Result.Content.ReadAsStringAsync().Result;
				return JsonConvert.DeserializeObject<T>(responseString);
			}
		}

		public override void Request(NodeInfo node_info, string name, object input_param)
		{
			using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(input_param));
				client.PostAsync(string.Format("http://{0}:{1}/{2}", node_info.URL, node_info.Port, name), content);
			}
		}
	}
}
