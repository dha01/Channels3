using Core.Model.InvokeMethods.Base.Methods.DataModel;
using Core.Model.InvokeMethods.Local.CSharp.Assembly.DataModel;

namespace Core.Model.InvokeMethods.Local.CSharp.Assembly.Service
{
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
		
		System.Reflection.Assembly GetAssembly(string path);

		void AddAssembly(System.Reflection.Assembly assembly);
	}
}
