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
		private List<NodeServerInfo> _nodeList;

		public CoordinationService()
		{
			_nodeList = new List<NodeServerInfo>();
		}

		public List<NodeServerInfo> GetAvailableNodeList()
		{
			return _nodeList;
		}

		public NodeServerInfo GetSuitableNode()
		{
			if (!_nodeList.Any())
			{
				throw new Exception("Отсутствуют доступные узлы.");
			}
			
			return _nodeList.First();
		}

		public void AddNode(NodeServerInfo node)
		{
			if (!_nodeList.Exists(x => x.Equals(node)))
			{
				_nodeList.Add(node);

				Console.WriteLine("Добавлен новый сервер: {0}:{1} {2}", node.URL, node.Port, node.ServerType);
			}
		}
	}
}
