using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Network.DataModel;

namespace Core.Model.Network.Service
{
	public interface ICoordinationService
	{
		List<NodeServerInfo> GetAvailableNodeList();
		NodeServerInfo GetSuitableNode();
		void AddNode(NodeServerInfo node);
	}
}
