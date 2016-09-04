using System;
using System.Collections.Generic;
using System.Linq;
using Core.Model.Data.DataModel;

namespace Core.Model.Data.Service
{
	/// <summary>
	/// Сервис хранения данных.
	/// </summary>
	/// <typeparam name="T">Тип хранимых данных.</typeparam>
	public class DataService<T> : IDataService<T> where T : DataBase
	{
		#region Fields

		/// <summary>
		/// Массив данных.
		/// </summary>
		private readonly Dictionary<Guid, T> _invokedDataDictionary;

		/// <summary>
		/// Массив содержащий все данные, которые зависят от значения.
		/// </summary>
		private readonly Dictionary<Guid, List<Guid>> _linkToChilds;

		#endregion

		#region Constructor

		/// <summary>
		/// Инициализирет словари.
		/// </summary>
		public DataService()
		{
			_invokedDataDictionary = new Dictionary<Guid, T>();
			_linkToChilds = new Dictionary<Guid, List<Guid>>();
		}

		#endregion

		#region Methods/Public

		/// <summary>
		/// Добавляет в хранилище новый объект с данными.
		/// </summary>
		/// <param name="data">Данные.</param>
		public void Add(T data)
		{
			foreach (var parent in _invokedDataDictionary.Where(x => data.InputIds != null && data.InputIds.Contains(x.Key)))
			{
				List<Guid> list;
				
				if (!_linkToChilds.ContainsKey(parent.Key))
				{
					list = new List<Guid>();
					_linkToChilds.Add(parent.Key, list);
				}
				else
				{
					list = _linkToChilds[parent.Key];
				}
				list.Add(data.Id);
			}
			_invokedDataDictionary.Add(data.Id, data);
		}

		/// <summary>
		/// Возвращает перичесления данных с указанными идентификаторами.
		/// </summary>
		/// <param name="guid">Перечисление идентификаторов данных.</param>
		/// <returns>Перичесления данных.</returns>
		public IEnumerable<T> Get(IEnumerable<Guid> guid)
		{
			return guid.Select(x => _invokedDataDictionary[x]);
		}

		/// <summary>
		/// Возвращает данные с указанным идентификатором.
		/// </summary>
		/// <param name="guid">Идентификатор данных.</param>
		/// <returns>Данные.</returns>
		public T Get(Guid guid)
		{
			if (_invokedDataDictionary.ContainsKey(guid))
			{
				return _invokedDataDictionary[guid];
			}
			
			return null;
		}
		
		/// <summary>
		/// Возвращает все дочерние данные.
		/// </summary>
		/// <param name="guid">Идентификатор родительских данных.</param>
		/// <returns>Список дочерних данных.</returns>
		public IEnumerable<Guid> GetChildIds(Guid guid)
		{
			if (_linkToChilds.ContainsKey(guid))
			{
				return _linkToChilds[guid];
			}
			
			return new List<Guid>();
		}

		/// <summary>
		/// Возвращает все дочерние данные.
		/// </summary>
		/// <param name="guid">Идентификатор родительских данных.</param>
		/// <returns>Список дочерних данных.</returns>
		public IEnumerable<T> GetChilds(Guid guid)
		{
			return Get(GetChildIds(guid));
		}

		#endregion
	}
}
