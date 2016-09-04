using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Invoke.Base.Service;
using Core.Model.Invoke.Local.CSharp.Service;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.Service;

namespace Core.Model.Methods.Base.Service
{
	public class MethodService : IMethodService
	{
		private Dictionary<string, MethodBase> _methodDictionary;


		private Dictionary<Guid, AssemblyFile> _assemblyDictionary;
		private Dictionary<Guid, Dictionary<Guid, MethodBase>> _methodByAssemblyDictionary;


		private readonly IAssemblyServiceFactory _assemblyServiceFactory;

		public MethodService(IAssemblyServiceFactory assembly_service)
		{
			_methodDictionary = new Dictionary<string, MethodBase>();
			_assemblyServiceFactory = assembly_service;
		}

		public void AddMethod(MethodBase method)
		{
			if (_methodDictionary.ContainsKey(method.FullPath))
			{
				return;
			}
			
			_methodDictionary.Add(method.FullPath, method);
		}

		/// <summary>
		/// Возвращает метод по идентификатору.
		/// </summary>
		/// <param name="guid">Идентификатор метода.</param>
		/// <returns>Сервис для исполнения.</returns>
		public MethodBase GetMethod(MethodBase method_base)
		{
			if (_methodDictionary.ContainsKey(method_base.FullPath))
			{
				return _methodDictionary[method_base.FullPath];
			}

			var method = _assemblyServiceFactory.GetAssemblyService(method_base).GetMethod(method_base);

			if (method == null)
			{
				return null;
			}

			AddMethod(method);

			return method;
		}
	}
}
