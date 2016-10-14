using System;
using System.Collections.Generic;
using System.Linq;
using Core.Model.Network.Base.DataModel;

namespace Core.Model.Network.Service
{
	/// <summary>
	/// Сервис координации.
	/// </summary>
	public class CoordinationService : ICoordinationService
	{
		/// <summary>
		/// Список известных серверов.
		/// </summary>
		private List<NodeServerInfo> _nodeList;

		private int _currentNodeCount = 0;

		public CoordinationService()
		{
			_nodeList = new List<NodeServerInfo>();
		}

		public List<NodeServerInfo> GetAvailableNodeList()
		{
			return _nodeList;
		}

		/// <summary>
		/// Возвращает подходящий вычислительный узел.
		/// </summary>
		/// <returns></returns>
		public NodeServerInfo GetSuitableNode()
		{
			if (!_nodeList.Any())
			{
				throw new Exception("Отсутствуют доступные узлы.");
			}

			_currentNodeCount++;

			return _nodeList[_currentNodeCount % _nodeList.Count];
		}

		/// <summary>
		/// Добавляет в список известных серверов новый новый узел.
		/// </summary>
		/// <param name="node">Данные о сервере.</param>
		public void AddNode(NodeServerInfo node)
		{
			if (!_nodeList.Exists(x => x.Equals(node)))
			{
				_nodeList.Add(node);
				Console.WriteLine("{0} {1} Добавлен новый сервер: {2}:{3} {4}", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp(), node.URL, node.Port, node.ServerType);
			}
		}
	}
}
