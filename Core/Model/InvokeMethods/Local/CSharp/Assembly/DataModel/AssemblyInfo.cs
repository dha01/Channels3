using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.InvokeMethods.Local.CSharp.Methods.DataModel;

namespace Core.Model.InvokeMethods.Local.CSharp.Assembly.DataModel
{
	public class AssemblyInfo
	{
		public Guid Id { get; set; }
		public byte[] Assembly { get; set; }
		public List<CSharpMethod> CSharpMethods { get; set; }
	}
}
