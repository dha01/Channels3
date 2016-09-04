using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Invoke.Local.CSharp.Service;
using MethodBase = Core.Model.Methods.Base.DomainModel.MethodBase;

namespace Core.Model.Methods.CSharp.DomainModel
{
	public class CSharpMethod : MethodBase
	{
		public override Type MethodType
		{
			get { return _methodType; }
		}

		public Type Type { get; set; }
		public MethodInfo MethodInfo { get; set; }

		public CSharpMethod()
		{
			_methodType = typeof (CSharpMethod);
		}
	}
}
