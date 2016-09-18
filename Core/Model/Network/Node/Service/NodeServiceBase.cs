using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.Base.DataModel;
using Core.Model.Network.DataModel;
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

		//protected ISendRequestService _sendRequestService;
		protected IDataService<DataInvoke> _dataService;
		protected IDataCollectorService _dataCollectorService;
		protected ICSharpAssemblyService _cSharpAssemblyService;
		protected IAssemblyServiceFactory _assemblyServiceFactory;
		protected IMethodService _methodService;
		protected ICoordinationService _coordinationService;
		protected IInvokeServiceFactory _invokeServiceFactory;

		protected IWebServerService _webServerService;

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
			_cSharpAssemblyService = new CSharpAssemblyService();
			_assemblyServiceFactory = new AssemblyServiceFactory(_cSharpAssemblyService);
			_methodService = new MethodService(_assemblyServiceFactory);
			_coordinationService = new CoordinationService();
			//_sendRequestService = new SendRequestService();

			_webServerService = web_server_service;
			_webServerService.InitWebMethods(this);
			_invokeServiceFactory = new InvokeServiceFactory(_methodService, _cSharpAssemblyService, _coordinationService, _dataService, _webServerService);
			_dataCollectorService = new DataCollectorService(invoke_type, _invokeServiceFactory, _dataService, _webServerService);
		}

		public NodeServiceBase(IWebServerService web_server_service, IDataService<DataInvoke> data_service, IDataCollectorService data_collector_service, ICSharpAssemblyService c_sharp_assembly_service)
		{
			_port = web_server_service.Port;
			//_sendRequestService = new SendRequestService();
			_dataService = data_service;
			_cSharpAssemblyService = c_sharp_assembly_service;
			_assemblyServiceFactory = new AssemblyServiceFactory(_cSharpAssemblyService);
			_methodService = new MethodService(_assemblyServiceFactory);
			_coordinationService = new CoordinationService();
			_dataCollectorService = data_collector_service;

			_webServerService = web_server_service;
			_webServerService.InitWebMethods(this);
			_invokeServiceFactory = new InvokeServiceFactory(_methodService, _cSharpAssemblyService, _coordinationService, _dataService, _webServerService);
		}

		public NodeServiceBase(IDataService<DataInvoke> data_service, ICSharpAssemblyService c_sharp_assembly_service, IAssemblyServiceFactory assembly_service_factory,
			IMethodService method_service, ICoordinationService coordination_service, IInvokeServiceFactory invoke_service_factory, IDataCollectorService data_collector_service, IWebServerService web_server_service)
		{
			_port = web_server_service.Port;
			//_sendRequestService = send_request_service;
			_dataService = data_service;
			_cSharpAssemblyService = c_sharp_assembly_service;
			_assemblyServiceFactory = assembly_service_factory;
			_methodService = method_service;
			_coordinationService = coordination_service;
			_invokeServiceFactory = invoke_service_factory;
			_dataCollectorService = data_collector_service;

			_webServerService = web_server_service;
			_webServerService.InitWebMethods(this);
		}

		#endregion

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
