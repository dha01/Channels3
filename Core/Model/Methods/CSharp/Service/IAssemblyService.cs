using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Methods.CSharp.DomainModel;

namespace Core.Model.Methods.CSharp.Service
{
	public interface IAssemblyService
	{
		Assembly GetAssemblyForMethod(CSharpMethod csharp_method);
	}
}
