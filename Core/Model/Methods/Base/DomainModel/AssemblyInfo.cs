using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Model.Methods.Base.DomainModel
{
	public class AssemblyInfo
	{
		public string Namespace { get; set; }
		public string Version { get; set; }

		[JsonIgnore]
		public string AssemblyPath
		{
			get { return string.Format("{0}{1}", Namespace, Version); }
		}
	}
}
