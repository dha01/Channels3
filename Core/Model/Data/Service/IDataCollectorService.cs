using System;
using Core.Model.Data.DataModel;

namespace Core.Model.Data.Service
{
	/// <summary>
	/// Интерфейс сервиса обработки входных данных.
	/// </summary>
	public interface IDataCollectorService
	{
		/// <summary>
		/// Новый узел для вычисления.
		/// </summary>
		/// <param name="invoked_data"></param>
		void Invoke(DataInvoke invoked_data);

		/// <summary>
		/// Возвращает результат вычисления вычисляемых данных с указанным идентфииктаором.
		/// </summary>
		/// <param name="guid">Идентфиикатор вычисляемых данных.</param>
		/// <returns>Результат вычисления.</returns>
		object Get(Guid guid);
	}
}
