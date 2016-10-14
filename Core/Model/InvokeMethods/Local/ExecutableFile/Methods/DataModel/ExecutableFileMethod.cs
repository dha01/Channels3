using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.InvokeMethods.Local.ExecutableFile.Methods.DataModel
{
	public class ExecutableFileMethod : Base.Methods.DataModel.MethodBase
	{
		/// <summary>
		/// Тип класса метода.
		/// </summary>
		public override Type MethodType
		{
			get { return _methodType; }
		}
		
		public string Path { get; set; }

		/// <summary>
		/// Заполняет тип метода.
		/// </summary>
		public ExecutableFileMethod()
		{
			_methodType = typeof(ExecutableFileMethod);
		}
	}
}
