using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Network.Base.DataModel;
using Core.Model.Network.Service;
using Newtonsoft.Json;

namespace Core.Model.Network.Base.Service
{
	public class UdpServerService : WebServerServiceBase, IUdpServerService
	{
		/// <summary>
		/// Порт для отправки сообщений по протоколу UDP.
		/// </summary>
		public static int UdpPingPort = GetRandomPort();


		public const string BROADCAST_ADDRESS = "224.0.0.0";
		public const int BROADCAST_PORT = 6666;

		public UdpServerService()
		{
		}

		public UdpServerService(int port)
		{
			Task.Run(() =>
			{
				UdpClient udp_client = new UdpClient(BROADCAST_PORT);
				udp_client.JoinMulticastGroup(IPAddress.Parse(BROADCAST_ADDRESS), 50);
				while (true)
				{
					try
					{
						ReceiveUdpMessage(udp_client);
					}
					catch (Exception ex)
					{
						Console.WriteLine("UdpServer.Start.Task: {0}", ex.Message);
					}
				}
			});
		}
		
		public override void AddWebMethod(string root, MethodInfo method)
		{
			_routes.Add(string.Format(@"/{0}/{1}", root, method.Name), method);
		}

		public override T Request<T>(NodeInfo node_info, string name, object input_param)
		{
			/*using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(input_param));
				var response = client.PostAsync(string.Format("http://{0}:{1}/{2}", node_info.URL, node_info.Port, name), content);
				var responseString = response.Result.Content.ReadAsStringAsync().Result;
				return JsonConvert.DeserializeObject<T>(responseString);
			}*/
			return JsonConvert.DeserializeObject<T>("");
		}

		public void BroadcastRequest(string name, object input_param)
		{
			Request(null, name, input_param);
		}

		public override void Request(NodeInfo node_info, string name, object input_param)
		{
			var json_input = JsonConvert.SerializeObject(input_param);
			var str = string.Format("{0}\n{1}\n", name, json_input);
			var message = Encoding.ASCII.GetBytes(str);
			SendUdpMessage(message, node_info);
		}

		/// <summary>
		/// Отправляет сообщение по протоколу UDP.
		/// </summary>
		/// <param name="message"></param>
		public static void SendUdpMessage(byte[] message, NodeInfo node_info)
		{
			if (node_info == null)
			{
				node_info = new NodeInfo
				{
					URL = BROADCAST_ADDRESS,
					Port = BROADCAST_PORT
				};
			}
			
			try
			{
				var sender = new UdpClient(UdpPingPort);
				sender.Connect(node_info.URL, node_info.Port);

				sender.Send(message, message.Length);
				sender.Close();
			}
			catch (SocketException e)
			{
				if (e.ErrorCode == 10048)
				{
					Console.WriteLine("UdpPing: Порт {0} занят.", UdpPingPort);
					UdpPingPort = GetRandomPort();
				}
				else
				{
					Console.WriteLine("UdpPing: На порту {0} возникло исключение: {1}", UdpPingPort, e.Message);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("UdpPing: На порту {0} возникло исключение: {1}", UdpPingPort, e.Message);
			}
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


		/// <summary>
		/// Принимает UDP сообщения.
		/// </summary>
		/// <param name="udp_client"></param>
		private void ReceiveUdpMessage(UdpClient udp_client)
		{
			IPEndPoint remote_ip = null;
			//while (!IsDisposed)
			//{
			var data = udp_client.Receive(ref remote_ip);

			var message = Encoding.ASCII.GetString(data).Split('\n');

			var name = message[0];
			var json_input = message[1];

			InvokeWebMethod(name, json_input);
		}

	}
}
