﻿using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.InvokeMethods.Base.Invoke.DataModel;
using Core.Model.InvokeMethods.Base.Invoke.Service;
using Core.Model.InvokeMethods.Base.Methods.Service;
using Core.Model.InvokeMethods.Local.CSharp.Assembly.Service;
using Core.Model.Network.Base.DataModel;
using Core.Model.Network.Base.Service;
using Core.Model.Network.Service;

namespace Core.Model.Network.Node.Service
{
	public class CoordinationNodeService : NodeServiceBase
	{
		private readonly INotificationService _notificationService;

		public CoordinationNodeService()
			: this(WebServerServiceBase.GetRandomPort())
		{
			
		}

		public CoordinationNodeService(int port)
			: this(new HttpServerService(port))
		{
			
		}

		public CoordinationNodeService(IWebServerService web_server_service)
			: base(web_server_service, InvokeType.Remote)
		{
			_notificationService = new NotificationService(new UdpServerService(UdpServerService.BROADCAST_PORT));
			_notificationService.AddAction(OnReceiveNotify);
		}

		public CoordinationNodeService(IWebServerService web_server_service, IDataService<DataInvoke> data_service, IDataCollectorService data_collector_service, IAssemblyService c_sharp_assembly_service)
			: base(web_server_service, data_service, data_collector_service, c_sharp_assembly_service)
		{
			_notificationService = new NotificationService(new UdpServerService(UdpServerService.BROADCAST_PORT));
			_notificationService.AddAction(OnReceiveNotify);
		}

		public CoordinationNodeService( IDataService<DataInvoke> data_service, IAssemblyService c_sharp_assembly_service,
			IMethodService method_service, ICoordinationService coordination_service, IInvokeServiceFactory invoke_service_factory, IDataCollectorService data_collector_service, IWebServerService web_server_service,
			INotificationService notification_service)
			: base(data_service, c_sharp_assembly_service, method_service, coordination_service, invoke_service_factory, data_collector_service, web_server_service)
		{
			_notificationService = notification_service;
			_notificationService.AddAction(OnReceiveNotify);
		}

		public void OnReceiveNotify(NodeServerInfo value)
		{
			switch (value.ServerType)
			{
				case ServerType.Client:
					AddNode(_webServerService, value, GetServerInfo());
					break;
				case ServerType.Invoke:
					_coordinationService.AddNode(value);
					break;
			}
		}

		public override NodeServerInfo GetServerInfo()
		{
			var node_server_info = base.GetServerInfo();
			node_server_info.ServerType = ServerType.Coordination;
			return node_server_info;
		}
	}
}
