using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Network.Base.DataModel;

namespace Core.Model.Network.Service
{
	/// <summary>
	/// Интерфейс сервиса координации.
	/// </summary>
	public interface ICoordinationService
	{
		List<NodeServerInfo> GetAvailableNodeList();

		void SetSituableNode(Guid id, NodeServerInfo node_server_info);
		NodeServerInfo GetSuitableNode(Guid id);
		NodeServerInfo GetSuitableNode(DataInvoke data_invoke);
		void AddNode(NodeServerInfo node);
	}
}
