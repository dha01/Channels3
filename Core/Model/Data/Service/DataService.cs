using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;

namespace Core.Model.Data.Service
{
	public class DataService<T> : IDataService<T> where T : DataBase
	{
		/// <summary>
		/// Массив данных.
		/// </summary>
		private Dictionary<Guid, T> _invokedDataDictionary;

		/// <summary>
		/// Массив содержащий все данные, которые зависят от значения.
		/// </summary>
		private Dictionary<Guid, List<Guid>> _linkToChilds;
		
		public DataService()
		{
			_invokedDataDictionary = new Dictionary<Guid, T>();
			_linkToChilds = new Dictionary<Guid, List<Guid>>();
		}

		/// <summary>
		/// Добавляет новое исполняемое значение.
		/// </summary>
		/// <param name="invoke_data">Исполняемое значение.</param>
		public void Add(T invoke_data)
		{
			foreach (var parent in _invokedDataDictionary.Where(x => invoke_data.InputIds != null && invoke_data.InputIds.Contains(x.Key)))
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
				list.Add(invoke_data.Id);
			}
			_invokedDataDictionary.Add(invoke_data.Id, invoke_data);
		}

		public IEnumerable<T> Get(IEnumerable<Guid> guid)
		{
			return guid.Select(x => _invokedDataDictionary[x]);
		}

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

		/// <summary>
		/// Возвращает заполненные исполняемые данные.
		/// </summary>
		/// <param name="data_invoke">Исполняемые данные.</param>
		/// <returns>Заполненные исполняемые данные.</returns>
		public DataInvokeFilled FillData(DataInvoke data_invoke)
		{
			var data = Get(data_invoke.InputIds).ToList();
			if (data.Any(x => x.DataState != DataState.Complite))
			{
				throw new Exception("Не все значения ещё были получены данных.");
			}
			return DataInvokeFilled.Fill(data_invoke, data.Select(x => x.Value).ToArray());
		}
	}
}
