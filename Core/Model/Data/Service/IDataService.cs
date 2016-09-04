using System;
using System.Collections.Generic;
using Core.Model.Data.DataModel;

namespace Core.Model.Data.Service
{
	/// <summary>
	/// Сервис для работы с данными.
	/// </summary>
	public interface IDataService<T>  where T : DataBase
	{
		/// <summary>
		/// Возвращает перичесления данных с указанными идентификаторами.
		/// </summary>
		/// <param name="guid">Перечисление идентификаторов данных.</param>
		/// <returns>Перичесления данных.</returns>
		IEnumerable<T> Get(IEnumerable<Guid> guid);

		/// <summary>
		/// Возвращает данные с указанным идентификатором.
		/// </summary>
		/// <param name="guid">Идентификатор данных.</param>
		/// <returns>Данные.</returns>
		T Get(Guid guid);
		
		/// <summary>
		/// Добавляет новое исполняемое значение.
		/// </summary>
		/// <param name="invoke_data">Исполняемое значение.</param>
		void Add(T invoke_data);
		
		/// <summary>
		/// Возвращает идентификаторы всех дочерних данных.
		/// </summary>
		/// <param name="guid">Идентификатор родительских данных.</param>
		/// <returns>Список дочерних данных.</returns>
		IEnumerable<Guid> GetChildIds(Guid guid);

		/// <summary>
		/// Возвращает все дочерние данные.
		/// </summary>
		/// <param name="guid">Идентификатор родительских данных.</param>
		/// <returns>Список дочерних данных.</returns>
		IEnumerable<T> GetChilds(Guid guid);
	}
}
