using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Network.DataModel
{
	public enum RequestType
	{
		/// <summary>
		/// Данные.
		/// </summary>
		Data,

		/// <summary>
		/// Запрос данных.
		/// </summary>
		GetDtata,

		/// <summary>
		/// Библиотека.
		/// </summary>
		Assembly,

		/// <summary>
		/// Запрос библиотеки.
		/// </summary>
		GetAssembly

	}
}
