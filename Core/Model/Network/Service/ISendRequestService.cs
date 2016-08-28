using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Network.DataModel;

namespace Core.Model.Network.Service
{
	public interface ISendRequestService
	{
		bool SendData(Node receive_node, DataInvoke data_invoke);

		DataInvoke GetData(Node receive_node, Guid guid);

		bool AddData(Node receive_node, DataInvoke data_invoke);
	}
}
