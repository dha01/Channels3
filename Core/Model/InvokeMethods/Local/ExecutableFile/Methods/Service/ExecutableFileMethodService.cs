using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.InvokeMethods.Base.Methods.DataModel;
using Core.Model.InvokeMethods.Base.Methods.Service;
using Core.Model.InvokeMethods.Local.ExecutableFile.Methods.DataModel;

namespace Core.Model.InvokeMethods.Local.ExecutableFile.Methods.Service
{
	public class ExecutableFileMethodService : IMethodService
	{
		/// <summary>
		/// Инициализирует сервис.
		/// </summary>
		/// <param name="assembly_service_factory">Фабрика сервиса пространства имен.</param>
		public ExecutableFileMethodService()
		{

		}

		/// <summary>
		/// Добавляет в хранилище новый метод.
		/// </summary>
		/// <param name="method"></param>
		public void AddMethod(MethodBase method)
		{
			throw new NotImplementedException("Метод ExecutableFileMethodService.AddMethod не реализован.");
		}

		/// <summary>
		/// Возвращает метод по идентификатору.
		/// </summary>
		/// <param name="method_base">Базовое описание метода.</param>
		/// <returns>Сервис для исполнения.</returns>
		public MethodBase GetMethod(MethodBase method_base)
		{
			var default_path = System.Configuration.ConfigurationManager.AppSettings["DefaultExecutableFilesPath"];
			
			var path = string.Format(@"{0}/{1}{2}",
				default_path, 
				string.IsNullOrEmpty(method_base.Namespace) ? "" : method_base.Namespace + "/", 
				method_base.MethodName);

			if (!File.Exists(path))
			{
				throw new Exception(string.Format(@"Неудалось найти файл '{0}'.", path));
			}

			return new ExecutableFileMethod
			{
				Path = path,
				Version = method_base.Version,
				InputParamsTypeNames = method_base.InputParamsTypeNames,
				TypeName = method_base.TypeName,
				MethodName = method_base.MethodName,
				Namespace = method_base.Namespace
			};
		}
	}
}
