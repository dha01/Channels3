using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Network.Base.DataModel;

namespace Core.Model.Network.Base.Service
{
	public interface INotificationService
	{
		void AddAction(Action<NodeServerInfo> action);

		void RunRegularNotify(NodeServerInfo value, int timeout);
	}
}
