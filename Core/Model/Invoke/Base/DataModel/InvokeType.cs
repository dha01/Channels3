using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Model.Invoke.Base.DataModel
{
	/// <summary>
	/// Тип исполнения.
	/// </summary>
	public enum InvokeType
	{
		/// <summary>
		/// Локально.
		/// </summary>
		Local,

		/// <summary>
		/// Удаленно.
		/// </summary>
		Remote,

		/// <summary>
		/// Автоматический выбор.
		/// </summary>
		Auto,

		/// <summary>
		/// Ручной выбор.
		/// </summary>
		Manual
	}
}
