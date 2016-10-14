using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Methods.CSharp.DomainModel
{
	public class AssemblyInfo
	{
		public Guid Id { get; set; }
		public byte[] Assembly { get; set; }
		public List<CSharpMethod> CSharpMethods { get; set; }
	}
}
