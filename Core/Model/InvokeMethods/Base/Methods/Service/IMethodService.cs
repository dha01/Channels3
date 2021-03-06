﻿using Core.Model.InvokeMethods.Base.Methods.DataModel;

namespace Core.Model.InvokeMethods.Base.Methods.Service
{
	/// <summary>
	/// Интерфейс сервиса для работы и хранения методов.
	/// </summary>
	public interface IMethodService
	{
		void AddMethod(MethodBase method);

		/// <summary>
		/// Возвращает метод по базовому описанию.
		/// </summary>
		/// <param name="method_base">Базовое описание метода.</param>
		/// <returns>Сервис для исполнения.</returns>
		MethodBase GetMethod(MethodBase method_base);
	}
}
