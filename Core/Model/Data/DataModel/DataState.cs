namespace Core.Model.Data.DataModel
{
	/// <summary>
	/// Состояние данных.
	/// </summary>
	public enum DataState
	{
		/// <summary>
		/// Неизвестно.
		/// </summary>
		Unknown,
		
		/// <summary>
		/// Не готовы к исполнению.
		/// </summary>
		NotReadyForInvoke,

		/// <summary>
		/// Готовы к исполнению
		/// </summary>
		ReadyForInvoke,

		/// <summary>
		/// Готовое значение.
		/// </summary>
		Complite
	}
}
