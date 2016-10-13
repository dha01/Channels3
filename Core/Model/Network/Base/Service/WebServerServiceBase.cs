using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Core.Model.Network.Base.DataModel;

namespace Core.Model.Network.Service
{
	/// <summary>
	/// Базовый класс сервиса приема и передачи данных по сети.
	/// </summary>
	[WebClass(Namespace = "Base")]
	public abstract class WebServerServiceBase : IWebServerService
	{
		/// <summary>
		/// Минимальное значение порта.
		/// </summary>
		protected const int PORT_MIN = 30000;

		/// <summary>
		/// Максимальное значение порта.
		/// </summary>
		protected const int PORT_MAX = 40000;

		/// <summary>
		/// Случайное значения порта.
		/// </summary>
		protected static Random Random = new Random(DateTime.Now.Millisecond);

		protected Dictionary<string, MethodInfo> _routes;

		protected Dictionary<MethodInfo, object> _invokedObjects;

		protected int _port;

		public int Port
		{
			get { return _port; }
		}

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
				throw new Exception(String.Format("{0}->Метод не найден.", GetType().Name));
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

		public static int GetRandomPort()
		{
			// Получаем произвольный порт из диапазона.
			for (var i = 0; i < (PORT_MAX - PORT_MIN) * 0.1; i++)
			{
				var port = Random.Next(PORT_MIN, PORT_MAX);

				try
				{
					Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					Socket.Bind(new IPEndPoint(IPAddress.Any, port));
					Socket.Close();
					return port;
				}
				catch (SocketException e)
				{
					if (e.ErrorCode == 10048)
					{
						Console.WriteLine("Порт {0} занят.", port);
					}
					else
					{
						Console.WriteLine("На порту {0} возникло исключение: {1}", port, e.Message);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("На порту {0} возникло исключение: {1}", port, e.Message);
				}
			}

			for (var i = PORT_MIN; i < PORT_MAX; i++)
			{
				var port = i;

				try
				{
					Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					Socket.Bind(new IPEndPoint(IPAddress.Any, port));
					Socket.Close();
					return port;
				}
				catch (SocketException e)
				{
					if (e.ErrorCode == 10048)
					{
						Console.WriteLine("Порт {0} занят.", port);
					}
					else
					{
						Console.WriteLine("На порту {0} возникло исключение: {1}", port, e.Message);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("На порту {0} возникло исключение: {1}", port, e.Message);
				}
			}
			throw new Exception(String.Format("Все порты в диапазоне от {0} до {1} заняты.", PORT_MIN, PORT_MAX));
		}
	}
}
