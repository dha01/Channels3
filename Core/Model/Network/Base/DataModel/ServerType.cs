namespace Core.Model.Network.Base.DataModel
{
	/// <summary>
	/// Тип сервера.
	/// </summary>
	public enum ServerType
	{
		/// <summary>
		/// Клиентский.
		/// </summary>
		Client,

		/// <summary>
		/// Координационный.
		/// </summary>
		Coordination,

		/// <summary>
		/// Исполняемый.
		/// </summary>
		Invoke,

		/// <summary>
		/// Неопределен.
		/// </summary>
		Undefined 
	}
}
