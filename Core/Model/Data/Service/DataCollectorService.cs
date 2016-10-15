using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.InvokeMethods.Base.Invoke.DataModel;
using Core.Model.InvokeMethods.Base.Invoke.Service;
using Core.Model.Network.Node.Service;
using Core.Model.Network.Service;

namespace Core.Model.Data.Service
{
	/// <summary>
	/// Сервис обработки входных данных.
	/// </summary>
	public class DataCollectorService : IDataCollectorService
	{
		#region Fields

		/// <summary>
		/// Очередь на исполнение.
		/// </summary>
		private readonly QueueInvoker<DataInvoke> _queueInvoker;

		/// <summary>
		/// Фабрика сервисов исполнения.
		/// </summary>
		private readonly IInvokeServiceFactory _invokeServiceFactory;

		/// <summary>
		/// Сервис для работы с данными.
		/// </summary>
		private readonly IDataService<DataInvoke> _dataService;

		//private readonly ISendRequestService _sendRequestService;

		private readonly ConcurrentDictionary<Guid, WaitResult> _requestedResults;

		private readonly IWebServerService _webServerService;

		private readonly ICoordinationService _coordinationService;

		/// <summary>
		/// Тип исполнения.
		/// </summary>
		private readonly InvokeType _invokeType;

		public bool InvokeNotReadyData { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Иничиализирует с указанными сервисами.
		/// </summary>
		/// <param name="invoke_type">Тип исполнения.</param>
		/// <param name="invoke_service_factory">Фабрика сервисов исполнения.</param>
		/// <param name="data_service">Сервис для работы с данными.</param>
		public DataCollectorService(InvokeType invoke_type, IInvokeServiceFactory invoke_service_factory, IDataService<DataInvoke> data_service, IWebServerService web_server_service, ICoordinationService coordination_service)
		{
			_invokeType = invoke_type;
			_queueInvoker = new QueueInvoker<DataInvoke>(OnDequeue);
			_invokeServiceFactory = invoke_service_factory;
			_invokeServiceFactory.AddOnDequeueEvent(OnAfterInvoke);
			_dataService = data_service;
			//_sendRequestService = send_request_service;
			_requestedResults = new ConcurrentDictionary<Guid, WaitResult>();

			_webServerService = web_server_service;

			_coordinationService = coordination_service;
		}

		#endregion

		#region Methods/Public

		/// <summary>
		/// Добавляет вычисляемые данные.
		/// </summary>
		/// <param name="invoked_data">Вычисляемые данные.</param>
		public void Invoke(DataInvoke invoked_data)
		{
			_queueInvoker.Enqueue(invoked_data);
		}

		public class WaitResult
		{
			public ManualResetEvent ManualResetEvent { get; set; }
			public Task RequestedData { get; set; }
		}

		/// <summary>
		/// Возвращает результат вычисления вычисляемых данных с указанным идентфииктаором.
		/// </summary>
		/// <param name="guid">Идентфиикатор вычисляемых данных.</param>
		/// <returns>Результат вычисления.</returns>
		public object Get(Guid guid)
		{
			var request_data = new DataInvoke(guid)
			{
				IsRequestData = true
			};

			var mer = new WaitResult
			{
				ManualResetEvent = new ManualResetEvent(false)
			};
			//var manual_reset_event = new ManualResetEvent(false);
			_requestedResults.TryAdd(guid, mer);

			Invoke(request_data);

			mer.ManualResetEvent.WaitOne();
			var result = _dataService.Get(guid);
			if (result == null || !result.HasValue)
			{
				throw new Exception("Ошибка при запросе данных.");
			}
			return result.Value;
		}

		#endregion

		#region Methods/Private

		/// <summary>
		/// Возвращает актуальное состояние для исполняемых данных.
		/// </summary>
		/// <param name="invoked_data">Исполняемые данные.</param>
		/// <returns>Состояние данных.</returns>
		private DataState GetState(DataInvoke invoked_data)
		{
			if (invoked_data.HasValue)
			{
				return DataState.Complite;
			}

			var data_list = new List<DataInvoke>();

			foreach (var id in invoked_data.InputIds)
			{
				var data = _dataService.Get(id);
				if (data == null)
				{
					data = NodeServiceBase.GetData(_webServerService, invoked_data.Sender, id);
					if (data == null)
					{
						throw new Exception("DataCollectorService.GetState Ошибк апри получении данных для выполения функции.");
					}
					_dataService.Add(data);
				}
				data_list.Add(data);
			}
			
			if (data_list.All(x => x.HasValue))
			{
				return DataState.ReadyForInvoke;
			}

			return DataState.NotReadyForInvoke;
		}

		/// <summary>
		/// Добавляет новые исполняемые данные.
		/// </summary>
		/// <param name="invoked_data">Исполняемые данные.</param>
		private void AddNewInvokedData(DataInvoke invoked_data)
		{
			invoked_data.DataState = GetState(invoked_data);
			_dataService.Add(invoked_data);
		}

		/// <summary>
		/// Событие после исполнения метода для исполняемых данных.
		/// </summary>
		/// <param name="invoked_data"></param>
		private void OnAfterInvoke(DataInvoke invoked_data)
		{
			Invoke(invoked_data);
		}

		/// <summary>
		/// Событие при извлечении из очереди.
		/// </summary>
		/// <param name="invoked_data"></param>
		private void OnDequeue(DataInvoke invoked_data)
		{
			try
			{
				var exists_value = _dataService.Get(invoked_data.Id);

				// Запрос на возвращение данных.
				if (invoked_data.IsRequestData)
				{
					if (exists_value == null || !exists_value.HasValue)
					{
						// TODO: нужно разобраться с причиной почему не работает как надо
						/*var wr = _requestedResults[invoked_data.Id];
						if (wr.RequestedData == null)
						{
							var node = _coordinationService.GetSuitableNode(invoked_data.Id);
							if (node != null)
							{
								wr.RequestedData = Task.Run(() =>
								{
									var r = NodeServiceBase.GetData(_webServerService, node, invoked_data.Id);
									exists_value.Value = r.Value;

									Console.WriteLine("{0} {1} Получен результат исполнения удаленного метода {2}: результат {3}",
										Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp(), r.Method.MethodName,
										r.Value);
									wr.ManualResetEvent.Set();
								});
							}
						}*/
						
						Invoke(invoked_data);
						return;
					}

					WaitResult requested_result;
					_requestedResults.TryRemove(invoked_data.Id, out requested_result);
					if (requested_result != null)
					{
						requested_result.ManualResetEvent.Set();
					}
					return;
				}

				// Запрос на добавление данных.
				if (exists_value == null)
				{
					// Новое значение.
					AddNewInvokedData(invoked_data);
				}
				else
				{
					// Смена состяния.
					var new_state = GetState(invoked_data);
					if (exists_value.DataState == new_state)
					{
						return;
					}
					exists_value.DataState = new_state;
				}

				// Действия в зависимости от состояния.
				switch (invoked_data.DataState)
				{
					case DataState.NotReadyForInvoke:
						if (InvokeNotReadyData)
						{
							_invokeServiceFactory.GetInvokeService(invoked_data, _invokeType).Invoke(invoked_data);
						}
						else
						{
							CheckNotReadyValue(invoked_data);
						}
						break;
					case DataState.ReadyForInvoke:
						_invokeServiceFactory.GetInvokeService(invoked_data, _invokeType).Invoke(invoked_data);
						break;
					case DataState.Complite:
						foreach (var data in _dataService.GetChilds(invoked_data.Id))
						{
							Invoke(data);
						}
						break;
				}
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("DataCollectorService->Неизвестная ошибка: {0}", e.Message));
			}
		}

		/// <summary>
		/// Проверяет наличие всех входных параметров в хранилище данных.
		/// Если в хранилище данных нет требуемого параметра, то запращивает их у отправителя вычисляемого значения.
		/// </summary>
		/// <param name="invoked_data">Вычиляемое значение.</param>
		private void CheckNotReadyValue(DataInvoke invoked_data)
		{
			// TODO: лучше запускать отдельным потоком. И добавить обработку ошибок.
			
			foreach (var id in invoked_data.InputIds)
			{
				var value = _dataService.Get(id);
				if (value == null)
				{
					if (invoked_data.Sender != null)
					{
						var data = NodeServiceBase.GetData(_webServerService, invoked_data.Sender, id);

						if (data == null)
						{
							throw new NotImplementedException("Запрошенные данные отсутствуют.");
						}

						_dataService.Add(data);
					}
					else
					{
						throw new NotImplementedException("Не указан отправитель.");
					}
				}
			}
		}

		#endregion
	}
}
