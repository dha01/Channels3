using System;
using System.Linq;
using System.Net.Sockets;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.DataModel;
using Core.Model.Network.Service;

namespace Core.Model.Server.Service
{
	/// <summary>
	/// Сервер исполнения.
	/// </summary>
	[WebClass(Namespace = "Default")]
	public class InvokeServerService :  IInvokeServerService
	{
		//private ISendRequestService _sendRequestService;
		private IDataService<DataInvoke> _dataService;
		private IDataCollectorService _dataCollectorService;
		private ICSharpAssemblyService _cSharpAssemblyService;
		private IAssemblyServiceFactory _assemblyServiceFactory;
		private IMethodService _methodService;
		private ICoordinationService _coordinationService;
		private IInvokeServiceFactory _invokeServiceFactory;

		private IWebServerService _webServerService;

		public InvokeServerService(int port)
			: this(new HttpServerService(port))
		{
			
		}

		public InvokeServerService(IWebServerService web_server_service)
		{
			//_sendRequestService = new SendRequestService();
			_dataService = new DataService<DataInvoke>();
			_cSharpAssemblyService = new CSharpAssemblyService();
			_assemblyServiceFactory = new AssemblyServiceFactory(_cSharpAssemblyService);
			_methodService = new MethodService(_assemblyServiceFactory);
			_coordinationService = new CoordinationService();
			//_sendRequestService = new SendRequestService();
			_invokeServiceFactory = new InvokeServiceFactory(_methodService, _cSharpAssemblyService, _coordinationService, _dataService, _webServerService);

			_webServerService = web_server_service;
			_webServerService.InitWebMethods(this);
			_dataCollectorService = new DataCollectorService(InvokeType.Local, _invokeServiceFactory, _dataService, _webServerService);
		}

		public InvokeServerService(IWebServerService web_server_service, IDataService<DataInvoke> data_service, IDataCollectorService data_collector_service, ICSharpAssemblyService c_sharp_assembly_service)
		{
		//	_sendRequestService = new SendRequestService();
			_dataService = data_service;
			_cSharpAssemblyService = c_sharp_assembly_service;
			_assemblyServiceFactory = new AssemblyServiceFactory(_cSharpAssemblyService);
			_methodService = new MethodService(_assemblyServiceFactory);
			_coordinationService = new CoordinationService();
			//_sendRequestService = new SendRequestService();
			_dataCollectorService = data_collector_service;

			_webServerService = web_server_service;
			_webServerService.InitWebMethods(this);
			_invokeServiceFactory = new InvokeServiceFactory(_methodService, _cSharpAssemblyService, _coordinationService, _dataService, _webServerService);
		}
	}
}
