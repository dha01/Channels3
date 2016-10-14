using System.Reflection;
using Core.Model.Network.Base.DataModel;

namespace Core.Model.Network.Service
{
	/// <summary>
	/// Интерфейс сервиса приема и передачи данных по сети.
	/// </summary>
	public interface IWebServerService
	{
		/// <summary>
		/// Порт.
		/// </summary>
		int Port { get; }

		/// <summary>
		/// Добавляет сетевой метод.
		/// </summary>
		/// <param name="root">Корень.</param>
		/// <param name="method">Метод.</param>
		void AddWebMethod(string root, MethodInfo method);

		/// <summary>
		/// Инициализирует сетевой метод.
		/// </summary>
		/// <param name="invoked_object">Объет содержащий сетевые методы.</param>
		void InitWebMethods(object invoked_object);

		/// <summary>
		/// Запрашивает исполнение сетевого метода на удаленном узле и ожидает результата.
		/// </summary>
		/// <typeparam name="T">Тип результата.</typeparam>
		/// <param name="node_info">Информация об удаленном сетевом узле.</param>
		/// <param name="name">Название метода.</param>
		/// <param name="input_param">Входной параметр.</param>
		/// <returns>Результат исполнения.</returns>
		T Request<T>(NodeInfo node_info, string name, object input_param);

		/// <summary>
		/// Запрашивает исполнение сетевого метода на удаленном узле без ожидания результата.
		/// </summary>
		/// <param name="node_info">Информация об удаленном сетевом узле.</param>
		/// <param name="name">Название метода.</param>
		/// <param name="input_param">Входной параметр.</param>
		void Request(NodeInfo node_info, string name, object input_param);
	}
}
