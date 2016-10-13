namespace Core.Model.Network.Base.DataModel
{
	/// <summary>
	/// Информация о сервере.
	/// </summary>
	public class NodeServerInfo : NodeInfo
	{
		/// <summary>
		/// Тип сервера.
		/// </summary>
		public ServerType ServerType { get; set; }

		/// <summary>
		/// Сравнение информации о серверах.
		/// </summary>
		/// <param name="node_server_info">Иснформация сравниваемого сервера.</param>
		/// <returns>Эквивалентны ли сервера.</returns>
		public bool Equals(NodeServerInfo node_server_info)
		{
			return ServerType == node_server_info.ServerType
			    && URL.Equals(node_server_info.URL)
			    && Port == node_server_info.Port;
		}
	}
}
