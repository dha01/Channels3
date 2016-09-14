using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Local.CSharp.Service;
using Core.Model.Invoke.Remote.Service;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.Service;

namespace Core.Model.Invoke.Base.Service
{
	/// <summary>
	/// Фабрика сервиса исполнения.
	/// </summary>
	public class InvokeServiceFactory : IInvokeServiceFactory
	{

		private readonly IMethodService _methodService;
		
		private readonly Dictionary<Type, IInvokeService> _serviceDictionary;

		private readonly IAssemblyService _assemblyService;

		private readonly ICoordinationService _coordinationService;

		//private readonly ISendRequestService _sendRequestService;

		private readonly IDataService<DataInvoke> _dataService;

		private readonly IWebServerService _webServerService;

		public void AddOnDequeueEvent(Action<DataInvoke> action)
		{
			foreach (var service in _serviceDictionary)
			{
				service.Value.OnAfterInvoke += action;
			}
		}

		/// <summary>
		/// Инициализирует сервисы.
		/// </summary>
		/// <param name="method_service"></param>
		/// <param name="assembly_service"></param>
		/// <param name="coordination_service"></param>
		/// <param name="send_request_service"></param>
		/// <param name="data_service"></param>
		public InvokeServiceFactory(IMethodService method_service, IAssemblyService assembly_service, ICoordinationService coordination_service,
			IDataService<DataInvoke> data_service, IWebServerService web_server_service)
		{
			_methodService = method_service;
			_assemblyService = assembly_service;
			_coordinationService = coordination_service;
			//_sendRequestService = send_request_service;
			_webServerService = web_server_service;

			var remote_invoke_service = new RemoteInvokeService(_coordinationService, _webServerService);
			var invoke_c_sharp_method = new InvokeCSharpService(_assemblyService, _methodService, data_service);
			
			_serviceDictionary = new Dictionary<Type, IInvokeService>
			{
				{typeof(RemoteInvokeService), remote_invoke_service},
				{typeof(InvokeCSharpService), invoke_c_sharp_method},
				{typeof(CSharpMethod), invoke_c_sharp_method}
			};
		}
		
		/// <summary>
		/// Возвращает подходящий сервис для исполнения.
		/// </summary>
		/// <param name="invoked_data">Исполняемые данные.</param>
		/// <param name="invoke_type">Тип исполнения.</param>
		/// <returns>Серис исполнения.</returns>
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
							return _serviceDictionary[_methodService.GetMethod(invoked_data.Method).GetType()];
					}
					break;
				case InvokeType.Remote:
					return _serviceDictionary[typeof(RemoteInvokeService)];
				case InvokeType.Local:
					return _serviceDictionary[_methodService.GetMethod(invoked_data.Method).GetType()];
			}

			throw new Exception(string.Format("InvokeServiceFactory.GetInvokeService -> Тип {0} недопустим.", invoked_data.InvokeType));
		}
	}
}
