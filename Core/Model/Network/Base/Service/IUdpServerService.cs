using Core.Model.Network.Service;

namespace Core.Model.Network.Base.Service
{
	/// <summary>
	/// Интерфейс сервиса приема и передачи данных по протоколу UDP.
	/// </summary>
	public interface IUdpServerService : IWebServerService
	{
		/// <summary>
		/// Отправляет широковещательный з.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="input_param"></param>
		void BroadcastRequest(string name, object input_param);
	}
}
