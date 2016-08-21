using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;

namespace Core.Model.Data.Service
{
	public class DataCollectorService : IDataCollectorService
	{
		#region Properties

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

		#endregion

		#region Constructor

		/// <summary>
		/// Инициализирует сервисы по умолчанию.
		/// </summary>
		public DataCollectorService()
			: this(new InvokeServiceFactory(), new DataService<DataInvoke>())
		{
		}

		/// <summary>
		/// Иничиализирует с указанными сервисами.
		/// </summary>
		/// <param name="invoke_service_factory">Фабрика сервисов исполнения.</param>
		/// <param name="data_service">Сервис для работы с данными.</param>
		public DataCollectorService(IInvokeServiceFactory invoke_service_factory, IDataService<DataInvoke> data_service)
		{
			_queueInvoker = new QueueInvoker<DataInvoke>(OnDequeue);
			_invokeServiceFactory = invoke_service_factory;
			_invokeServiceFactory.AddOnDequeueEvent(OnAfterInvoke);
			_dataService = data_service;
		}

		#endregion

		#region Methods / Public

		/// <summary>
		/// Новый узел для вычисления.
		/// </summary>
		/// <param name="invoked_data"></param>
		public void Invoke(DataInvoke invoked_data)
		{
			_queueInvoker.Enqueue(invoked_data);
		}

		#endregion

		#region Methods / Private

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

			if (_dataService.Get(invoked_data.InputIds).All(x => x.HasValue))
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
			invoked_data.DataState = DataState.Complite;
			Invoke(invoked_data);
		}

		/// <summary>
		/// Событие при извлечении из очереди.
		/// </summary>
		/// <param name="invoked_data"></param>
		private void OnDequeue(DataInvoke invoked_data)
		{
			var exists_value = _dataService.Get(invoked_data.Id);

			if (exists_value == null)
			{
				AddNewInvokedData(invoked_data);
			}
			else
			{
				var new_state = GetState(invoked_data);
				if (exists_value.DataState == new_state)
				{
					return;
				}
				exists_value.DataState = new_state;
			}

			switch (invoked_data.DataState)
			{
				case DataState.NotReadyForInvoke:
					throw new NotImplementedException();
					break;
				case DataState.ReadyForInvoke:
					_invokeServiceFactory.GetInvokeService(invoked_data).Invoke(_dataService.FillData(invoked_data));
					break;
				case DataState.Complite:
					foreach (var data in _dataService.GetChilds(invoked_data.Id))
					{
						Invoke(data);
					}
					break;
			}
		}

		#endregion
	}
}
