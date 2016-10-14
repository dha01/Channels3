using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.Base.Service;
using MethodBase = System.Reflection.MethodBase;

namespace Core.Model.Methods.CSharp.Service
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
		Base.DomainModel.MethodBase GetMethod(Base.DomainModel.MethodBase method_base);

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
		
		Assembly GetAssembly(string path);

		void AddAssembly(Assembly assembly);
	}
}
