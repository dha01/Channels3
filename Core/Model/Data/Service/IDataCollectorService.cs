using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;

namespace Core.Model.Data.Service
{
	public interface IDataCollectorService
	{
		/// <summary>
		/// Новый узел для вычисления.
		/// </summary>
		/// <param name="invoked_data"></param>
		void Invoke(DataInvoke invoked_data);
	}
}
