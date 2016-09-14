using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Network.DataModel;

namespace Core.Model.Network.Service
{
	public class CoordinationService : ICoordinationService
	{
		private List<NodeInfo> _nodeList;

		public CoordinationService()
		{
			_nodeList = new List<NodeInfo>();
		}

		public List<NodeInfo> GetAvailableNodeList()
		{
			return _nodeList;
		}

		public NodeInfo GetSuitableNode()
		{
			if (!_nodeList.Any())
			{
				throw new Exception("Отсутствуют доступные узлы.");
			}
			
			return _nodeList.First();
		}

		public void AddNode(NodeInfo node)
		{
			_nodeList.Add(node);
		}

	}
}
