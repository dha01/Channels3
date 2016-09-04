using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Invoke.Base.Service;
using Newtonsoft.Json;

namespace Core.Model.Methods.Base.DomainModel
{
	/// <summary>
	/// Базовое описание метода.
	/// </summary>
	public class MethodBase : AssemblyInfo
	{
		protected Type _methodType;
		public virtual Type MethodType
		{
			get { return _methodType; }
			set { _methodType = value; }
		}

		public string TypeName { get; set; }
		public string MethodName { get; set; }
		public string[] InputParamsTypeNames { get; set; }

		[JsonIgnore]
		public string MethodPath
		{
			get { return string.Format("{0}{1}{2}", TypeName, MethodName, string.Join(",", InputParamsTypeNames.ToList())); }
		}

		[JsonIgnore]
		public string FullPath
		{
			get { return string.Format("{0}{1}", AssemblyPath, MethodPath); }
		}

		
		/*public Guid Id { get; set; }

		public Guid AssemblyFileId { get; set; }

		public string TypeName { get; set; }

		public string MethodName { get; set; }
		public string[] InputParamsTypeNames { get; set; }*/
	}
}
