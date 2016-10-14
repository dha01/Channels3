using System;
using Core.Model.Data.DataModel;
using Core.Model.InvokeMethods.Base.Invoke.DataModel;
using Core.Model.InvokeMethods.Base.Invoke.Service;
using Core.Model.Network.Base.DataModel;
using Core.Model.Network.Node.Service;
using Core.Model.Network.Service;

namespace Core.Model.InvokeMethods.Remote.Service
{
	/// <summary>
	/// Сервис удаленного исполнения.
	/// </summary>
	public class RemoteInvokeService : InvokeServiceBase
	{
		#region Fields

		/// <summary>
		/// Сервиса приема и передачи данных по сети.
		/// </summary>
		private readonly IWebServerService _webServerService;

		/// <summary>
		/// Сервис координации.
		/// </summary>
		private readonly ICoordinationService _coordinationService;

		#endregion

		#region Constructor

		/// <summary>
		/// Устанавливает сервисы по умолчанию
		/// </summary>
		public RemoteInvokeService()
			:this(new CoordinationService(), new HttpServerService())
		{
			
		}

		/// <summary>
		/// Устанавливает указанные сервисы.
		/// </summary>
		/// <param name="coordination_service">Сервис координации.</param>
		/// <param name="web_server_service">Сервиса приема и передачи данных по сети.</param>
		public RemoteInvokeService(ICoordinationService coordination_service, IWebServerService web_server_service)
		{
			_coordinationService = coordination_service;
			_webServerService = web_server_service;
		}

		#endregion

		#region Methods/Private

		/// <summary>
		/// Тип исполнения.
		/// </summary>
		protected override InvokeType InvokeType
		{
			get
			{
				return InvokeType.Remote;
			}
		}

		/// <summary>
		/// Исполнение метода.
		/// </summary>
		/// <param name="invoked_data">Исполняемые данные.</param>
		/// <param name="callback">Функция, вызываемая по окончанию исполнения.</param>
		protected override void InvokeMethod(DataInvoke invoked_data, Action<DataInvoke> callback)
		{
			NodeServerInfo node;
			
			try
			{
				node = _coordinationService.GetSuitableNode();
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("RemoteInvokeService->InvokeMethod Ошибка при получении узла для исполнения удаленного метода: {0}", e.Message));
			}
			
			try
			{
				NodeServiceBase.AddData(_webServerService, node, invoked_data);
				
				var result = NodeServiceBase.GetData(_webServerService, node, invoked_data.Id);
				invoked_data.Value = result.Value;

				Console.WriteLine("{0} {1} Получен результат исполнения удаленного метода {2}: результат {3}", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp(), invoked_data.Method.MethodName, invoked_data.Value);
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("RemoteInvokeService->InvokeMethod Ошибка при вызове удаленного метода: {0}", e.Message));
			}

			try
			{
				callback.Invoke(invoked_data);
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("RemoteInvokeService->InvokeMethod Ошибка при вызове события по завершению исполнения: {0}", e.Message));
			}
		}

		#endregion
	}
}
