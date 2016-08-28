using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Network.DataModel;
using Newtonsoft.Json;

namespace Core.Model.Network.Service
{
	public class ReceiveRequestService<T> : IReceiveRequestService<T> where T: new()
	{
		private const int DEFAULT_PORT = 1234;
		
		public int Port = 80;

		HttpListener Listener; // Объект, принимающий TCP-клиентов

		private HttpServerBase _httpServerBase;

		public ReceiveRequestService()
			: this(DEFAULT_PORT)
		{
			
		}

		public ReceiveRequestService(int port)
		{
			Port = port;
			_httpServerBase = new HttpServerBase(Port);

			var methods = new Dictionary<string, WebAction>();
			var web_action = new WebAction();
			//web_action.SetAction<DataInvoke>(NewData);
			methods.Add("NewData", web_action);

			_httpServerBase.UrlPaths.Add("Default", methods);
		}

		public void NewData(DataInvoke data_invoke)
		{
			//new DataCollectorService().Invoke(data_invoke);
		}

		public Action<T> OnReceive { get; set; }


		// Остановка сервера
		~ReceiveRequestService()
		{
			// Если "слушатель" был создан
			if (Listener != null)
			{
				// Остановим его
				Listener.Stop();
			}
		}
	}
}
