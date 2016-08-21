using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;

namespace Core.Model.Data.DataModel
{
	/// <summary>
	/// Данные для исполнения.
	/// </summary>
	public class DataInvoke : DataBase
	{
		/// <summary>
		/// Тип исполнения.
		/// </summary>
		public InvokeType? InvokeType { get; set; }

		public DataInvoke(Guid id, object value)
			: base(id, value)
		{
		}

		public DataInvoke(Guid id)
			: base(id)
		{
		}

		public DataInvoke() 
			: this(Guid.NewGuid())
		{
		}
		public DataInvoke(object value)
			: this(Guid.NewGuid(), value)
		{
		}
	}
}
