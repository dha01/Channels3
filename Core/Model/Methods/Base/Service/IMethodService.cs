using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Network.DataModel;

namespace Core.Model.Methods.Base.Service
{
	public interface IMethodService
	{
		void AddMethod(MethodBase method);

		/// <summary>
		/// Возвращает метод по идентификатору.
		/// </summary>
		/// <param name="guid">Идентификатор метода.</param>
		/// <returns>Сервис для исполнения.</returns>
		MethodBase GetMethod(MethodBase method_base);
	}
}
