using Core.Model.Methods.Base.DomainModel;

namespace Core.Model.Methods.Base.Service
{
	/// <summary>
	/// Интерфейс сервиса для работы с библиотеками.
	/// </summary>
	public interface IAssemblyService
	{
		/// <summary>
		/// Возвращает файл с библиотекой.
		/// </summary>
		/// <param name="full_assembly_name">Полное имя файла.</param>
		/// <returns>Файл с библиотекой.</returns>
		AssemblyFile GetAssemblyFile(string full_assembly_name);

		/// <summary>
		/// Возвращает метод по базовому описанию.
		/// </summary>
		/// <param name="method_base">Базовое описание метода.</param>
		/// <returns>Метод.</returns>
		MethodBase GetMethod(MethodBase method_base);

		/// <summary>
		/// Добавляет библиотеку.
		/// </summary>
		/// <param name="assembly_file">Файл с библиотекой.</param>
		void AddAssembly(AssemblyFile assembly_file);

		/// <summary>
		/// Добавляет библиотеку.
		/// </summary>
		/// <param name="path">Путь к файлу с библиотекой.</param>
		void AddAssembly(string path);
	}
}
