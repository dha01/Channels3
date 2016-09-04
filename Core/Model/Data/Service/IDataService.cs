using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;

namespace Core.Model.Data.Service
{
	/// <summary>
	/// Сервис для работы с данными.
	/// </summary>
	public interface IDataService<T>  where T : DataBase
	{
		IEnumerable<T> Get(IEnumerable<Guid> guid);

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

		/// <summary>
		/// Возвращает заполненные исполняемые данные.
		/// </summary>
		/// <param name="data_invoke">Исполняемые данные.</param>
		/// <returns>Заполненные исполняемые данные.</returns>
	/*	DataInvokeFilled FillData(DataInvoke data_invoke);*/
	}
}
