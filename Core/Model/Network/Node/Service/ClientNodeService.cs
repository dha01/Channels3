using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.Base.DataModel;
using Core.Model.Network.Base.Service;
using Core.Model.Network.DataModel;
using Core.Model.Network.Service;
using Core.Model.Server.Service;

namespace Core.Model.Network.Node.Service
{
	public class ClientNodeService : NodeServiceBase, IClientNodeService
	{
		private readonly INotificationService _notificationService;
		
		public ClientNodeService(int port)
			: this(new HttpServerService(port))
		{

			
		}

		public ClientNodeService(IWebServerService web_server_service)
			: base(web_server_service, InvokeType.Remote)
		{
			_notificationService = new NotificationService(new UdpServerService());

			var node = new NodeServerInfo
			{
				URL = WebServerServiceBase.GetLocalIp(),
				Port = web_server_service.Port,
				ServerType = ServerType.Client
			};
			_notificationService.RunRegularNotify(node, 5000);
		}

		public ClientNodeService(IWebServerService web_server_service, IDataService<DataInvoke> data_service, IDataCollectorService data_collector_service, ICSharpAssemblyService c_sharp_assembly_service, INotificationService notification_service)
			: base(web_server_service, data_service, data_collector_service, c_sharp_assembly_service)
		{
			_notificationService = notification_service;
		}

		public ClientNodeService(IDataService<DataInvoke> data_service, ICSharpAssemblyService c_sharp_assembly_service, IAssemblyServiceFactory assembly_service_factory,
			IMethodService method_service, ICoordinationService coordination_service, IInvokeServiceFactory invoke_service_factory, IDataCollectorService data_collector_service, IWebServerService web_server_service)
			: base(data_service, c_sharp_assembly_service, assembly_service_factory, method_service, coordination_service, invoke_service_factory, data_collector_service, web_server_service)
		{

		}

		public void Invoke(DataInvoke data_invoke)
		{
			data_invoke.Sender = GetServerInfo();
			_dataCollectorService.Invoke(data_invoke);
		}

		public object Get(Guid guid)
		{
			return _dataCollectorService.Get(guid);
		}

		public override NodeServerInfo GetServerInfo()
		{
			var node_server_info = base.GetServerInfo();
			node_server_info.ServerType = ServerType.Client;
			return node_server_info;
		}
	}
}
