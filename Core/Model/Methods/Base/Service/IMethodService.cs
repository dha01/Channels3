using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.DomainModel;

namespace Core.Model.Methods.Base.Service
{
	public interface IMethodService
	{
		/// <summary>
		/// Возвращает сервис для исполнения метода.
		/// </summary>
		/// <param name="guid">Идентификатор метода.</param>
		/// <returns>Сервис для исполнения.</returns>
		IInvokeService GetInvokeService(Guid guid);

		void AddMethod(MethodBase method);

		/// <summary>
		/// Возвращает метод по идентификатору.
		/// </summary>
		/// <param name="guid">Идентификатор метода.</param>
		/// <returns>Сервис для исполнения.</returns>
		MethodBase GetMethod(Guid guid);
	}
}
