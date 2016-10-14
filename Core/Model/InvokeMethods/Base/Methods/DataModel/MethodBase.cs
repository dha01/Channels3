using System;
using System.Linq;
using Newtonsoft.Json;

namespace Core.Model.InvokeMethods.Base.Methods.DataModel
{
	/// <summary>
	/// Базовое описание исполняемого метода.
	/// </summary>
	public class MethodBase : BinaryFileInfoBase
	{
		/// <summary>
		/// Тип класса метода.
		/// </summary>
		protected Type _methodType;

		/// <summary>
		/// Тип класса метода.
		/// </summary>
		public virtual Type MethodType
		{
			get { return _methodType; }
			set { _methodType = value; }
		}

		/// <summary>
		/// Название типа.
		/// </summary>
		public string TypeName { get; set; }

		/// <summary>
		/// Название метода.
		/// </summary>
		public string MethodName { get; set; }

		/// <summary>
		/// Название типов входных параметров.
		/// </summary>
		public string[] InputParamsTypeNames { get; set; }

		/// <summary>
		/// Строка для описания метода.
		/// </summary>
		[JsonIgnore]
		public string MethodPath
		{
			get { return string.Format("{0}{1}{2}", TypeName, MethodName, string.Join(",", InputParamsTypeNames.ToList())); }
		}

		/// <summary>
		/// Строка с полным описанием метода.
		/// </summary>
		[JsonIgnore]
		public string FullPath
		{
			get { return string.Format("{0}{1}", AssemblyPath, MethodPath); }
		}
	}
}
