using Core.Model.InvokeMethods.Base.Methods.DataModel;

namespace Core.Model.InvokeMethods.Local.CSharp.Assembly.DataModel
{
	/// <summary>
	/// Файл с библиотекой.
	/// </summary>
	public class AssemblyFile : BinaryFileInfoBase
	{
		/// <summary>
		/// Бинарный файл библиотеки.
		/// </summary>
		public byte[] Data { get; set; }
	}
}
