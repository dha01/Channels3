using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;

namespace Core.Model.Data.DataModel
{
	/// <summary>
	/// Данные для исполнения.
	/// </summary>
	public class DataInvokeFilled : DataInvoke
	{
		public object[] Inputs { get; set; }

		public DataInvokeFilled(Guid id, object value)
			: base(id, value)
		{
		}

		public DataInvokeFilled(Guid id)
			: base(id)
		{
		}

		public DataInvokeFilled() 
			: this(Guid.NewGuid())
		{
		}
		public DataInvokeFilled(object value)
			: this(Guid.NewGuid(), value)
		{
		}

	/*	public static DataInvokeFilled Fill(DataInvoke data_invoke, object[] values)
		{
			return new DataInvokeFilled(data_invoke.Id)
			{
				DataState = data_invoke.DataState,
				Inputs = values,
				MethodId = data_invoke.MethodId,
				InputIds = data_invoke.InputIds
			};
		}*/
	}
}
