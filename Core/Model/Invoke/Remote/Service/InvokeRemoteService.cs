using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Network.Node.Service;
using Core.Model.Network.Service;
using Core.Model.Server.Service;

namespace Core.Model.Invoke.Remote.Service
{
	public class RemoteInvokeService : InvokeServiceBase
	{
		private readonly IWebServerService _webServerService;
		private readonly ICoordinationService _coordinationService;

		public RemoteInvokeService()
			:this(new CoordinationService(), new HttpServerService())
		{
			
		}

		public RemoteInvokeService(ICoordinationService coordination_service, IWebServerService web_server_service)
		{
			_coordinationService = coordination_service;
			_webServerService = web_server_service;
		}
		
		protected override InvokeType InvokeType
		{
			get
			{
				return InvokeType.Remote;
			}
		}

		protected override void InvokeMethod(DataInvoke invoked_data, Action<DataInvoke> callback)
		{
			var node = _coordinationService.GetSuitableNode();

			NodeServiceBase.AddData(_webServerService, node, invoked_data);

			var result = NodeServiceBase.GetData(_webServerService, node, invoked_data.Id);
			invoked_data.Value = result.Value;

			Console.WriteLine("{0} {1} Получен результат исполнения удаленного метода {2}: результат {3}", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp(), invoked_data.Method.MethodName, invoked_data.Value);
			
			callback.Invoke(invoked_data);
		}
	}
}
