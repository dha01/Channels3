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

		Assembly GetAssemblyById(Guid guid);

	/*	Guid AddAssembly(byte[] assembly_bytes);

		void AddAssembly(Guid guid, byte[] assembly_bytes);

		Guid AddAssembly(Type type);*/

		List<CSharpMethod> GetMethods(Type type);

		Guid AddMethod(MethodInfo method);
	}
}
