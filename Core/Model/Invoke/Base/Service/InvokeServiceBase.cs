using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;

namespace Core.Model.Invoke.Base.Service
{
	/// <summary>
	/// Базовый класс сервиса исполнения.
	/// </summary>
	public abstract class InvokeServiceBase : IInvokeService
	{
		#region Fields

		/// <summary>
		/// Очередь на исполнение.
		/// </summary>
		private readonly QueueInvoker<DataInvokeFilled> _queueInvoker;

		/// <summary>
		/// Событие после исполнения.
		/// </summary>
		public Action<DataInvokeFilled> OnAfterInvoke { get; set; }

		/// <summary>
		/// Тип исполнения.
		/// </summary>
		protected virtual InvokeType InvokeType
		{
			get
			{
				return InvokeType.Manual;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Инициализирует очердь на исполнение.
		/// </summary>
		protected InvokeServiceBase()
		{
			_queueInvoker = new QueueInvoker<DataInvokeFilled>(OnDequeue);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Отправка данных на исполнение.
		/// </summary>
		/// <param name="invoked_data">Данные для исполнения.</param>
		public void Invoke(DataInvokeFilled invoked_data)
		{
			switch (InvokeType)
			{
				case InvokeType.Manual:
					break;
				case InvokeType.Local:
				case InvokeType.Remote:
					invoked_data.InvokeType = InvokeType;
					break;
				case InvokeType.Auto:
					invoked_data.InvokeType = GetAutoInvokeType();
					break;
			}

			_queueInvoker.Enqueue(invoked_data);
		}

		/// <summary>
		/// Исполнение метода.
		/// </summary>
		/// <param name="invoked_data"></param>
		/// <param name="callback"></param>
		protected abstract void InvokeMethod(DataInvokeFilled invoked_data, Action<DataInvokeFilled> callback);
		
		/// <summary>
		/// Событие при извлечении из очереди.
		/// </summary>
		/// <param name="invoked_data">Данные для исполнения.</param>
		private void OnDequeue(DataInvokeFilled invoked_data)
		{
			InvokeMethod(invoked_data, OnAfterInvoke);
		}

		/// <summary>
		/// Автоматический выбор типа исполнения.
		/// </summary>
		/// <returns></returns>
		protected InvokeType GetAutoInvokeType()
		{
			return InvokeType.Local;
		}

		#endregion
	}
}
