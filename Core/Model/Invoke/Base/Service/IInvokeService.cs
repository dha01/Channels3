using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;

namespace Core.Model.Invoke.Base.Service
{
	public interface IInvokeService
	{
		/// <summary>
		/// Событие после исполнения.
		/// </summary>
		Action<DataInvokeFilled> OnAfterInvoke { get; set; }
		
		/// <summary>
		/// Отправка данных на исполнение.
		/// </summary>
		/// <param name="invoked_data">Данные для исполнения.</param>
		void Invoke(DataInvokeFilled invoked_data);
	}
}
