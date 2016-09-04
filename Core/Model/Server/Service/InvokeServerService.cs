using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.DataModel;
using Core.Model.Network.Service;

namespace Core.Model.Server.Service
{
	/// <summary>
	/// Сервер исполнения.
	/// </summary>
	public class InvokeServerService : IInvokeServerService
	{
		private readonly ISendRequestService _sendRequestService;
		//private readonly IReceiveRequestService<Request> _receiveRequestService;
		private readonly IDataService<DataInvoke> _dataService;
		private readonly IDataCollectorService _dataCollectorService;

		private readonly IAssemblyService _assemblyService;


		private readonly HttpServerBase _httpServerBase;
		/*
		public InvokeServerService()
			: this(new SendRequestService(), new DataService<DataInvoke>(), new DataCollectorService(InvokeType.Auto), new AssemblyService(), new HttpServerBase())
		{

		}*/

		public InvokeServerService(ISendRequestService send_request_service, /*IReceiveRequestService<Request> receive_request_service,*/ IDataService<DataInvoke> data_service, 
			IDataCollectorService data_collector_service, IAssemblyService assembly_service, HttpServerBase http_server_base)
		{
			_sendRequestService = send_request_service;
			//_receiveRequestService = receive_request_service;
			_dataService = data_service;
			_dataCollectorService = data_collector_service;
			_assemblyService = assembly_service;

			//_receiveRequestService.OnReceive += OnReceive;


			_httpServerBase = http_server_base;
			

			var methods = new Dictionary<string, WebAction>();
			var web_action = new WebAction()
			{
				InputType = typeof(Guid),
				MethodInfo = typeof(InvokeServerService).GetMethod("GetData"),
				Object = this
			};
			methods.Add("GetData", web_action);

			web_action = new WebAction()
			{
				InputType = typeof(DataInvoke),
				MethodInfo = typeof(InvokeServerService).GetMethod("AddData"),
				Object = this
			};
			methods.Add("AddData", web_action);

			_httpServerBase.UrlPaths.Add("Default", methods);
			/*
			web_action = new WebAction()
			{
				InputType = typeof(Guid),
				MethodInfo = typeof(CSharpAssemblyInfo).GetMethod("GetCSharpAssemblyInfoByMethodId"),
				Object = this
			};
			methods.Add("GetAssemblyFile", web_action);

			_httpServerBase.UrlPaths.Add("Default", methods);*/
		}
		/*
		public AssemblyFile GetAssemblyFile(Guid assembly_file_id)
		{
			return _assemblyService.GetAssemblyFile(assembly_file_id);
		}*/

		public DataInvoke GetData(Guid guid)
		{
			_dataCollectorService.Get(guid);
			return _dataService.Get(guid);
		}

		public bool AddData(DataInvoke data_invoke)
		{
			_dataCollectorService.Invoke(data_invoke);
			return true;
		}

		public AssemblyFile GetAssembly(AssemblyInfo assembly_info)
		{
			var assembly = _assemblyService.GetAssemblyFile(assembly_info.AssemblyPath);
			return assembly;
		}

		public bool AddAssembly(AssemblyFile assembly_file)
		{
			_assemblyService.AddAssembly(assembly_file);
			return true;
		}
		/*
		private void OnReceive(Request request)
		{
			Request response = null;
			
			switch (request.RequestType)
			{
				case RequestType.GetDtata:
					response = new Request()
					{
						RequestType = RequestType.Data,
						Data = _dataService.Get((Guid)request.Data),
						Sender = null
					};
					break;
				case RequestType.Data:
					_dataCollectorService.Invoke((DataInvoke)response.Data);
					break;
				case RequestType.GetAssembly:
					_assemblyService.GetAssemblyById((Guid) request.Data);
					break;
				case RequestType.Assembly:
					_assemblyService.AddAssembly((byte[])request.Data);
					break;
			}


			_sendRequestService.Send(response, request.Sender);
		}*/
	}
}
