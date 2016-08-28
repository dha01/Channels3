using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Invoke.Base.Service;
using Core.Model.Invoke.Local.CSharp.Service;
using Core.Model.Methods.Base.DomainModel;

namespace Core.Model.Methods.Base.Service
{
	public class MethodService : IMethodService
	{
		private Dictionary<Guid, MethodBase> _methodDictionary;

		public MethodService()
		{
			_methodDictionary = new Dictionary<Guid, MethodBase>();
		}
		
		/// <summary>
		/// Возвращает сервис для исполнения метода.
		/// </summary>
		/// <param name="guid">Идентификатор метода.</param>
		/// <returns>Сервис для исполнения.</returns>
		public IInvokeService GetInvokeService(Guid guid)
		{
			return new InvokeCSharpService();
		}

		public void AddMethod(MethodBase method)
		{
			_methodDictionary.Add(method.Id, method);
		}

		/// <summary>
		/// Возвращает метод по идентификатору.
		/// </summary>
		/// <param name="guid">Идентификатор метода.</param>
		/// <returns>Сервис для исполнения.</returns>
		public MethodBase GetMethod(Guid guid)
		{
			if (_methodDictionary.ContainsKey(guid))
			{
				return _methodDictionary[guid];
			}
			return null;
		}
	}
}
