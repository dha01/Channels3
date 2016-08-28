using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Network.Service;
using Core.Model.Server.Service;

namespace Core.Model.Invoke.Remote.Service
{
	public class RemoteInvokeService : InvokeServiceBase
	{
		private readonly ISendRequestService _sendRequestService;
		private readonly ICoordinationService _coordinationService;

		public RemoteInvokeService()
			:this(new CoordinationService(), new SendRequestService())
		{
			
		}

		public RemoteInvokeService(ICoordinationService coordination_service, ISendRequestService send_request_service)
		{
			_coordinationService = coordination_service;
			_sendRequestService = send_request_service;
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

			_sendRequestService.AddData(node, invoked_data);

			var result = _sendRequestService.GetData(node, invoked_data.Id);
			invoked_data.Value = result.Value;

			callback.Invoke(invoked_data);
		}
	}
}
