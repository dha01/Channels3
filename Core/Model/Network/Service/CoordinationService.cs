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
		private List<Node> _nodeList;

		public CoordinationService()
		{
			_nodeList = new List<Node>();
		}
		
		public List<Node> GetAvailableNodeList()
		{
			return _nodeList;
		}

		public Node GetSuitableNode()
		{
			if (!_nodeList.Any())
			{
				throw new Exception("Отсутствуют доступные узлы.");
			}
			
			return _nodeList.First();
		}

		public void AddNode(Node node)
		{
			_nodeList.Add(node);
		}
	}
}
