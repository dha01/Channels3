using System;
using System.Reflection;
using MethodBase = Core.Model.InvokeMethods.Base.Methods.DataModel;

namespace Core.Model.InvokeMethods.Local.CSharp.Methods.DataModel
{
	/// <summary>
	/// Исполняемый метод C#.
	/// </summary>
	public class CSharpMethod : MethodBase.MethodBase
	{
		/// <summary>
		/// Тип класса метода.
		/// </summary>
		public override Type MethodType
		{
			get { return _methodType; }
		}

		/// <summary>
		/// Тип класса для инициализации объекта.
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		/// Информация о методе C#.
		/// </summary>
		public MethodInfo MethodInfo { get; set; }

		/// <summary>
		/// Заполняет тип метода.
		/// </summary>
		public CSharpMethod()
		{
			_methodType = typeof (CSharpMethod);
		}
	}
}
