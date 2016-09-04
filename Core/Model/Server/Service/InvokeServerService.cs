using System;
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
	public class InvokeServerService : HttpServerService, IInvokeServerService
	{
		/*private readonly IDataService<DataInvoke> _dataService;
		private readonly IDataCollectorService _dataCollectorService;
		private readonly IAssemblyService _assemblyService;
		*/
		private ISendRequestService _sendRequestService;
		private IDataService<DataInvoke> _dataService;
		private IDataCollectorService _dataCollectorService;
		public ICSharpAssemblyService _cSharpAssemblyService;
		private IAssemblyServiceFactory _assemblyServiceFactory;
		private IMethodService _methodService;
		public ICoordinationService _coordinationService;
		private IInvokeServiceFactory _invokeServiceFactory;

		public InvokeServerService(string url)
			: base(url)
		{
			_sendRequestService = new SendRequestService();
			_dataService = new DataService<DataInvoke>();
			_cSharpAssemblyService = new CSharpAssemblyService();
			_assemblyServiceFactory = new AssemblyServiceFactory(_cSharpAssemblyService);
			_methodService = new MethodService(_assemblyServiceFactory);
			_coordinationService = new CoordinationService();
			_sendRequestService = new SendRequestService();
			_invokeServiceFactory = new InvokeServiceFactory(_methodService, _cSharpAssemblyService, _coordinationService, _sendRequestService, _dataService);
			_dataCollectorService = new DataCollectorService(InvokeType.Local, _invokeServiceFactory, _dataService, _sendRequestService);
		}

		public InvokeServerService(string url, IDataService<DataInvoke> data_service, IDataCollectorService data_collector_service, ICSharpAssemblyService c_sharp_assembly_service)
			: base(url)
		{
			_sendRequestService = new SendRequestService();
			_dataService = data_service;
			_cSharpAssemblyService = c_sharp_assembly_service;
			_assemblyServiceFactory = new AssemblyServiceFactory(_cSharpAssemblyService);
			_methodService = new MethodService(_assemblyServiceFactory);
			_coordinationService = new CoordinationService();
			_sendRequestService = new SendRequestService();
			_invokeServiceFactory = new InvokeServiceFactory(_methodService, _cSharpAssemblyService, _coordinationService, _sendRequestService, _dataService);
			_dataCollectorService = data_collector_service;
		}

		[WebMethod]
		public DataInvoke GetData(Guid guid)
		{
			_dataCollectorService.Get(guid);
			return _dataService.Get(guid);
		}

		[WebMethod]
		public bool AddData(DataInvoke data_invoke)
		{
			_dataCollectorService.Invoke(data_invoke);
			return true;
		}
		/*
		[WebMethod]
		public AssemblyFile GetAssembly(AssemblyInfo assembly_info)
		{
			var assembly = _assemblyService.GetAssemblyFile(assembly_info.AssemblyPath);
			return assembly;
		}

		[WebMethod]
		public bool AddAssembly(AssemblyFile assembly_file)
		{
			_assemblyService.AddAssembly(assembly_file);
			return true;
		}*/
	}
}
