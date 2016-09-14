using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Network.DataModel;
using Core.Model.Network.Service;

namespace Core.Model.Network.Base.Service
{
	public interface IUdpServerService : IWebServerService
	{
		void BroadcastRequest(string name, object input_param);
	}
}
