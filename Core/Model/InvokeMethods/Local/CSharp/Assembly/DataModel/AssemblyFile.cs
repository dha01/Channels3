namespace Core.Model.Methods.Base.DomainModel
{
	/// <summary>
	/// Файл с библиотекой.
	/// </summary>
	public class AssemblyFile : AssemblyInfo
	{
		/// <summary>
		/// Бинарный файл библиотеки.
		/// </summary>
		public byte[] Data { get; set; }
	}
}
