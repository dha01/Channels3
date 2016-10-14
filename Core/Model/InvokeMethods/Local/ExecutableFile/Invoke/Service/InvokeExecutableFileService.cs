using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.InvokeMethods.Base.Invoke.DataModel;
using Core.Model.InvokeMethods.Base.Invoke.Service;
using Core.Model.InvokeMethods.Base.Methods.DataModel;
using Core.Model.InvokeMethods.Base.Methods.Service;
using Core.Model.InvokeMethods.Local.ExecutableFile.Methods.DataModel;

namespace Core.Model.InvokeMethods.Local.ExecutableFile.Invoke.Service
{
	class InvokeExecutableFileService : InvokeServiceBase
	{
		#region Fields

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
		/// <param name="method_service">Сервис для работы с методами.</param>
		/// <param name="data_service">Сервис для работы с данными.</param>
		public InvokeExecutableFileService(IMethodService method_service, IDataService<DataInvoke> data_service)
		{
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
		private ExecutableFileMethod GetMethod(MethodBase method)
		{
			var result = (ExecutableFileMethod)_methodService.GetMethod(method);
			if (result == null)
			{
				result = (ExecutableFileMethod)method;
				_methodService.AddMethod(method);
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
			ExecutableFileMethod method;
			
			try
			{
				method = GetMethod(invoked_data.Method);
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("InvokeExecutableFileService->InvokeMethod Ошибка при получении метода: {0}", e.Message));
			}
			
			try
			{
				var is_os_windows =
					Environment.OSVersion.Platform == PlatformID.Win32NT ||
					Environment.OSVersion.Platform == PlatformID.Win32S ||
					Environment.OSVersion.Platform == PlatformID.Win32Windows ||
					Environment.OSVersion.Platform == PlatformID.WinCE;

				string arguments = "";
					
				if (invoked_data.InputIds.Any())
				{
					arguments = _dataService.Get(invoked_data.InputIds.First()).Value.ToString();
				}
				// Устанавливаем параметры запуска подпрограммы.
				var proc = new Process
				{
					StartInfo =
					{
						FileName = method.Path,
						Arguments = arguments,
						RedirectStandardOutput = true,
						CreateNoWindow = is_os_windows,
						UseShellExecute = !is_os_windows
					}
				};

				// Запускаем и ожидаем окончания выполнения.
				proc.Start();
				using (var reader = proc.StandardOutput)
				{
					string result = reader.ReadToEnd();
					invoked_data.Value = result;
				}
				proc.WaitForExit();
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
				throw new Exception(string.Format("InvokeExecutableFileService->InvokeMethod Ошибка при вызове события по завершению исполнения: {0}", e.Message));
			}
		}

		#endregion
	}
}
