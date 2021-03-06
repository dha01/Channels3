﻿using System;
using System.Collections.Generic;
using Core.Model.InvokeMethods.Base.Methods.DataModel;
using Core.Model.InvokeMethods.Base.Methods.Service;
using Core.Model.InvokeMethods.Local.CSharp.Assembly.DataModel;
using Core.Model.InvokeMethods.Local.CSharp.Assembly.Service;

namespace Core.Model.InvokeMethods.Local.CSharp.Methods.Service
{
	/// <summary>
	/// Сервис для работы и хранения методов.
	/// </summary>
	public class CSharpMethodService : IMethodService
	{
		private readonly Dictionary<string, MethodBase> _methodDictionary;


		private Dictionary<Guid, AssemblyFile> _assemblyDictionary;
		private Dictionary<Guid, Dictionary<Guid, MethodBase>> _methodByAssemblyDictionary;

		private readonly IAssemblyService _cSharpAssemblyService;

		/// <summary>
		/// Инициализирует сервис.
		/// </summary>
		/// <param name="assembly_service_factory">Фабрика сервиса пространства имен.</param>
		public CSharpMethodService(IAssemblyService c_sharp_assembly_service)
		{
			_methodDictionary = new Dictionary<string, MethodBase>();
			//_assemblyServiceFactory = assembly_service_factory;

			_cSharpAssemblyService = c_sharp_assembly_service;
		}

		/// <summary>
		/// Добавляет в хранилище новый метод.
		/// </summary>
		/// <param name="method"></param>
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

			var method = _cSharpAssemblyService.GetMethod(method_base);

			if (method == null)
			{
				return null;
			}

			AddMethod(method);

			return method;
		}
	}
}
