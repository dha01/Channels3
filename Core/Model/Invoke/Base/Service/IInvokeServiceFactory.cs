using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;

namespace Core.Model.Invoke.Base.Service
{
	/// <summary>
	/// Интерфейс фабрики сервисов исполнения.
	/// </summary>
	public interface IInvokeServiceFactory
	{
		void AddOnDequeueEvent(Action<DataInvoke> action);

		IInvokeService GetInvokeService(DataInvoke invoked_data);
	}
}
