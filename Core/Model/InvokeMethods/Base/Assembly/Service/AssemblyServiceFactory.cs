using System;
using System.Collections.Generic;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Methods.CSharp.Service;

namespace Core.Model.Methods.Base.Service
{
	/// <summary>
	/// Фабрика сервисов библиотек.
	/// </summary>
	public class AssemblyServiceFactory : IAssemblyServiceFactory
	{
		#region Fields

		/// <summary>
		/// Словарь сервисов библиотек.
		/// </summary>
		private Dictionary<Type, IAssemblyService> _assemblyServices;

		#endregion

		#region Constructor

		/// <summary>
		/// Устанавливает указанные сервисы.
		/// </summary>
		/// <param name="c_sharp_assembly_service">Сервис для работы с библиотеками C#.</param>
		public AssemblyServiceFactory(ICSharpAssemblyService c_sharp_assembly_service)
		{
			_assemblyServices = new Dictionary<Type, IAssemblyService>
			{
				{typeof (CSharpMethod), c_sharp_assembly_service}
			};
		}

		#endregion

		#region Methods/Public

		/// <summary>
		/// Возвращает сервис библиотек по базовому описанию исполняемого метода.
		/// </summary>
		/// <param name="method_base">Базовое описание исполняемого метода.</param>
		/// <returns>Сервис библиотек.</returns>
		public IAssemblyService GetAssemblyService(MethodBase method_base)
		{
			var method_type = method_base.MethodType;
			
			if (_assemblyServices.ContainsKey(method_type))
			{
				return _assemblyServices[method_type];
			}

			throw new Exception(string.Format("Тип {0} недопустим.", method_type));
		}

		#endregion
	}
}
