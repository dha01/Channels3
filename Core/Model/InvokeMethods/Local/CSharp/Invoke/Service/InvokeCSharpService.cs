using System;
using System.Linq;
using System.Reflection;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.Service;

namespace Core.Model.Invoke.Local.CSharp.Service
{
	/// <summary>
	/// Сервис исполнения методов C#.
	/// </summary>
	public class InvokeCSharpService : InvokeServiceBase
	{
		#region Fields

		/// <summary>
		/// Сервис для работы с библиотеками.
		/// </summary>
		private readonly IAssemblyService _assemblyService;

		/// <summary>
		/// Сервис для работы с методами.
		/// </summary>
		private readonly IMethodService _methodService;

		/// <summary>
		/// Сервис для работы с данными.
		/// </summary>
		private readonly IDataService<DataInvoke> _dataService;
		
		/// <summary>
		/// Тип исполнения.
		/// </summary>
		protected override InvokeType InvokeType
		{
			get
			{
				return InvokeType.Local;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Инициализирует сервисы.
		/// </summary>
		/// <param name="assembly_service">Сервис для работы с библиотеками.</param>
		/// <param name="method_service">Сервис для работы с методами.</param>
		/// <param name="data_service">Сервис для работы с данными.</param>
		public InvokeCSharpService(IAssemblyService assembly_service, IMethodService method_service, IDataService<DataInvoke> data_service)
		{
			_assemblyService = assembly_service;
			_methodService = method_service;
			_dataService = data_service;
		}

		#endregion

		#region Methods / Private

		/// <summary>
		/// Возвращает исполняемый метод C# исходя из базовой информации о методе.
		/// </summary>
		/// <param name="method">Базовое описание исполняемого метода.</param>
		/// <returns>Исполняемый метод C#.</returns>
		private CSharpMethod GetMethod(Methods.Base.DomainModel.MethodBase method)
		{
			var result = (CSharpMethod)_methodService.GetMethod(method);
			if (result == null)
			{
				result = (CSharpMethod)method;
				_methodService.AddMethod(method);
			}

			if (result.MethodInfo == null)
			{
				var assembly_file = _assemblyService.GetAssemblyFile(method.AssemblyPath);

				if (assembly_file == null)
				{
					throw new Exception(string.Format("InvokeCSharpService -> Библиотека не найдена: {0}", method.AssemblyPath));
				}

				var assembly = Assembly.Load(assembly_file.Data);
				var type = assembly.GetType(method.TypeName);
				result.MethodInfo = type.GetMethod(method.MethodName, method.InputParamsTypeNames.Select(Type.GetType).ToArray());
			}

			return result;
		}

		/// <summary>
		/// Исполняет метод и сохраняет полученный результат.
		/// </summary>
		/// <param name="invoked_data">Исполняемые данные.</param>
		/// <param name="callback">Событие при завершении исполнения.</param>
		protected override void InvokeMethod(DataInvoke invoked_data, Action<DataInvoke> callback)
		{
			CSharpMethod method;
			
			try
			{
				method = GetMethod(invoked_data.Method);
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("InvokeCSharpService->InvokeMethod Ошибка при получении метода: {0}", e.Message));
			}
			
			try
			{
				var inputs = invoked_data.InputIds.Select(x => _dataService.Get(x).Value).ToArray();
				var obj = Activator.CreateInstance(method.Type);
				invoked_data.Value = method.MethodInfo.Invoke(obj, inputs);
				Console.WriteLine("{0} {1} Исполнен метод {2}: результат {3}", Environment.GetEnvironmentVariables()["SLURM_PROCID"], WebServerServiceBase.GetLocalIp(), invoked_data.Method.MethodName, invoked_data.Value);
			}
			catch (Exception e)
			{
				invoked_data.Value = e.InnerException;
			}

			try
			{
				callback.Invoke(invoked_data);
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("InvokeCSharpService->InvokeMethod Ошибка при вызове события по завершению исполнения: {0}", e.Message));
			}
		}

		#endregion
	}
}
