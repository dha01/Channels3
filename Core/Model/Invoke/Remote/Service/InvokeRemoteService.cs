using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;

namespace Core.Model.Invoke.Remote.Service
{
	public class RemoteInvokeService : InvokeServiceBase
	{
		protected override InvokeType InvokeType
		{
			get
			{
				return InvokeType.Remote;
			}
		}

		protected override void InvokeMethod(DataInvokeFilled invoked_data, Action<DataInvokeFilled> callback)
		{
			throw new NotImplementedException();
			callback.Invoke(invoked_data);
		}
	}
}
