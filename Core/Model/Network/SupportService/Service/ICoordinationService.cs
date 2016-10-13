using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Network.Base.DataModel;

namespace Core.Model.Network.Service
{
	/// <summary>
	/// Интерфейс сервиса координации.
	/// </summary>
	public interface ICoordinationService
	{
		List<NodeServerInfo> GetAvailableNodeList();
		NodeServerInfo GetSuitableNode();
		void AddNode(NodeServerInfo node);
	}
}
