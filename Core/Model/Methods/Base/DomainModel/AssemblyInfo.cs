using Newtonsoft.Json;

namespace Core.Model.Methods.Base.DomainModel
{
	/// <summary>
	/// Информация о библиотеке.
	/// </summary>
	public class AssemblyInfo
	{
		/// <summary>
		/// Название пространства имен.
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// Версия.
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// Строка с путем к файлу по которому его можно идентифицировать.
		/// </summary>
		[JsonIgnore]
		public string AssemblyPath
		{
			get { return string.Format("{0}{1}", Namespace, Version); }
		}
	}
}
