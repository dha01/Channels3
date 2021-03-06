﻿using System;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.InvokeMethods.Base.Invoke.DataModel;
using Core.Model.InvokeMethods.Base.Invoke.Service;
using Core.Model.InvokeMethods.Base.Methods.Service;
using Core.Model.InvokeMethods.Local.CSharp.Assembly.Service;
using Core.Model.InvokeMethods.Local.CSharp.Methods.Service;
using Core.Model.InvokeMethods.Remote.Service;
using Core.Model.Network.Base.DataModel;
using Core.Model.Network.Service;
using Newtonsoft.Json;

namespace Core.Model.Network.Node.Service
{
	[WebClass(Namespace = NAMESPACE)]
	public class NodeServiceBase : INodeService
	{
		public const string NAMESPACE = "Default";

		private int _port;

		#region Fields

		/// <summary>
		/// Сервис для работы с данными.
		/// </summary>
		protected IDataService<DataInvoke> _dataService;

		/// <summary>
		/// Сервис обработки входных данных.
		/// </summary>
		protected IDataCollectorService _dataCollectorService;

		/// <summary>
		/// Сервис для работы с библиотеками C#.
		/// </summary>
		protected IAssemblyService _cSharpAssemblyService;

		/// <summary>
		/// Сервис для работы и хранения методов.
		/// </summary>
		protected IMethodService _methodService;

		/// <summary>
		/// Сервис координации.
		/// </summary>
		protected ICoordinationService _coordinationService;

		/// <summary>
		/// Фабрика сервисов исполнения.
		/// </summary>
		protected IInvokeServiceFactory _invokeServiceFactory;

		/// <summary>
		/// Сервис приема и передачи данных по сети.
		/// </summary>
		protected IWebServerService _webServerService;

		/// <summary>
		/// Сервис приема и передачи данных по сети.
		/// </summary>
		protected RemoteInvokeService _remoteInvokeService;

		#endregion

		#region Constructor

		public NodeServiceBase(int port)
			: this(new HttpServerService(port), InvokeType.Auto)
		{
			_port = port;
		}

		public NodeServiceBase(IWebServerService web_server_service, InvokeType invoke_type)
		{
			_port = web_server_service.Port;
			//_sendRequestService = new SendRequestService();
			_dataService = new DataService<DataInvoke>();
			_cSharpAssemblyService = new AssemblyService();
			//_assemblyServiceFactory = new AssemblyServiceFactory(_cSharpAssemblyService);
			_methodService = new CSharpMethodService(_cSharpAssemblyService);
			_coordinationService = new CoordinationService();
			//_sendRequestService = new SendRequestService();

			_webServerService = web_server_service;
			_webServerService.InitWebMethods(this);
			_remoteInvokeService = new RemoteInvokeService(_coordinationService, _webServerService);

			_invokeServiceFactory = new InvokeServiceFactory(_methodService, _cSharpAssemblyService, _coordinationService, _dataService, _webServerService, _remoteInvokeService);
			_dataCollectorService = new DataCollectorService(invoke_type, _invokeServiceFactory, _dataService, _webServerService, _coordinationService);
		}

		public NodeServiceBase(IWebServerService web_server_service, IDataService<DataInvoke> data_service, IDataCollectorService data_collector_service, IAssemblyService c_sharp_assembly_service)
		{
			_port = web_server_service.Port;
			//_sendRequestService = new SendRequestService();
			_dataService = data_service;
			_cSharpAssemblyService = c_sharp_assembly_service;
			//_assemblyServiceFactory = new AssemblyServiceFactory(_cSharpAssemblyService);
			_methodService = new CSharpMethodService(_cSharpAssemblyService);
			_coordinationService = new CoordinationService();
			_dataCollectorService = data_collector_service;

			_webServerService = web_server_service;
			_webServerService.InitWebMethods(this);
			_remoteInvokeService = new RemoteInvokeService(_coordinationService, _webServerService);

			_invokeServiceFactory = new InvokeServiceFactory(_methodService, _cSharpAssemblyService, _coordinationService, _dataService, _webServerService, _remoteInvokeService);
		}

		public NodeServiceBase(IDataService<DataInvoke> data_service, IAssemblyService c_sharp_assembly_service,
			IMethodService method_service, ICoordinationService coordination_service, IInvokeServiceFactory invoke_service_factory, IDataCollectorService data_collector_service, IWebServerService web_server_service)
		{
			_port = web_server_service.Port;
			//_sendRequestService = send_request_service;
			_dataService = data_service;
			_cSharpAssemblyService = c_sharp_assembly_service;
			_methodService = method_service;
			_coordinationService = coordination_service;
			_invokeServiceFactory = invoke_service_factory;
			_dataCollectorService = data_collector_service;

			_webServerService = web_server_service;
			_webServerService.InitWebMethods(this);
		}

		#endregion

		/// <summary>
		/// Возвращает информацию по узлу срвера.
		/// </summary>
		/// <returns></returns>
		public virtual NodeServerInfo GetServerInfo()
		{
			return new NodeServerInfo()
			{
				Port = _port,
				URL = WebServerServiceBase.GetLocalIp(),
				ServerType = ServerType.Undefined
			};
		}

		#region WebMethods

		[WebMethod]
		public DataInvoke GetData(Guid guid)
		{
			_dataCollectorService.Get(guid);
			return _dataService.Get(guid);
		}

		public static DataInvoke GetData(IWebServerService web_server_service, NodeInfo node_info, Guid guid)
		{
			return web_server_service.Request<DataInvoke>(node_info, string.Format("{0}/{1}", NAMESPACE, "GetData"), guid);
		}

		[WebMethod]
		public bool AddData(DataInvoke data_invoke)
		{
			_dataCollectorService.Invoke(data_invoke);
			return true;
		}

		public static bool AddData(IWebServerService web_server_service, NodeInfo node_info, DataInvoke data_invoke)
		{
			return web_server_service.Request<bool>(node_info, string.Format("{0}/{1}", NAMESPACE, "AddData"), data_invoke);
		}

		[WebMethod]
		public bool AddNode(NodeServerInfo node_server_info)
		{
			_coordinationService.AddNode(node_server_info);
			return true;
		}

		public static bool AddNode(IWebServerService web_server_service, NodeInfo node_info, NodeServerInfo node_server_info)
		{
			return web_server_service.Request<bool>(node_info, string.Format("{0}/{1}", NAMESPACE, "AddNode"), node_server_info);
		}

		#endregion
	}
}
