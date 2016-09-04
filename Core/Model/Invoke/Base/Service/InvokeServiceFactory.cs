using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Local.CSharp.Service;
using Core.Model.Invoke.Remote.Service;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.Service;

namespace Core.Model.Invoke.Base.Service
{
	public class InvokeServiceFactory : IInvokeServiceFactory
	{

		private readonly IMethodService _methodService;
		
		private readonly Dictionary<Type, IInvokeService> _serviceDictionary;

		private readonly IAssemblyService _assemblyService;

		private readonly ICoordinationService _coordinationService;

		private readonly ISendRequestService _sendRequestService;

		private readonly IDataService<DataInvoke> _dataService; 
		/*
		public InvokeServiceFactory()
			: this(new MethodService(), new AssemblyService(), new CoordinationService(), new SendRequestService(), new DataService<DataInvoke>())
		{
		}*/

		public void AddOnDequeueEvent(Action<DataInvoke> action)
		{
			foreach (var service in _serviceDictionary)
			{
				service.Value.OnAfterInvoke += action;
			}
		}

		public InvokeServiceFactory(IMethodService method_service, IAssemblyService assembly_service, ICoordinationService coordination_service,
			ISendRequestService send_request_service, IDataService<DataInvoke> data_service)
		{
			_methodService = method_service;
			_assemblyService = assembly_service;
			_coordinationService = coordination_service;
			_sendRequestService = send_request_service;

			_serviceDictionary = new Dictionary<Type, IInvokeService>
			{
				{typeof(RemoteInvokeService), new RemoteInvokeService(_coordinationService, _sendRequestService)},
				{typeof(InvokeCSharpService), new InvokeCSharpService(_assemblyService, _methodService, data_service)}
			};
		}
		
		public IInvokeService GetInvokeService(DataInvoke invoked_data, InvokeType invoke_type = InvokeType.Manual)
		{
			switch (invoke_type)
			{
				case InvokeType.Manual:
					switch (invoked_data.InvokeType)
					{
						case InvokeType.Remote:
							return _serviceDictionary[typeof(RemoteInvokeService)];
						case InvokeType.Local:
							return _serviceDictionary[_methodService.GetMethod(invoked_data.Method).InvokeServiceType];
					}
					break;
				case InvokeType.Remote:
					return _serviceDictionary[typeof(RemoteInvokeService)];
				case InvokeType.Local:
					return _serviceDictionary[_methodService.GetMethod(invoked_data.Method).InvokeServiceType];
			}
			
			
			throw new Exception(string.Format("Тип {0} недопустим.", invoked_data.InvokeType));
		}
	}
}
