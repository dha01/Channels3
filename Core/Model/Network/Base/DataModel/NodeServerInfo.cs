using Core.Model.Network.Base.DataModel;

namespace Core.Model.Network.DataModel
{
	public class NodeServerInfo : NodeInfo
	{
		public ServerType ServerType { get; set; }

		public bool Equals(NodeServerInfo node_server_info)
		{
			return ServerType == node_server_info.ServerType
			    && URL.Equals(node_server_info.URL)
			    && Port == node_server_info.Port;
		}
	}
}
