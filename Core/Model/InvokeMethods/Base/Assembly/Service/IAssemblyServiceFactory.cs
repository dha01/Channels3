using Core.Model.Methods.Base.DomainModel;

namespace Core.Model.Methods.Base.Service
{
	/// <summary>
	/// Интерфейс фабрики сервисов библиотек.
	/// </summary>
	public interface IAssemblyServiceFactory
	{
		/// <summary>
		/// Возвращает пространство имен для метода.
		/// </summary>
		/// <param name="method_base">Исполняемый метод.</param>
		/// <returns>Сервис пространства имен.</returns>
		IAssemblyService GetAssemblyService(MethodBase method_base);
	}
}
