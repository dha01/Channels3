using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.DomainModel;
using MethodBase = Core.Model.Methods.Base.DomainModel.MethodBase;

namespace Core.Model.Methods.CSharp.Service
{
	public interface IAssemblyService
	{
		AssemblyFile GetAssemblyFile(string path);

		Assembly GetAssembly(string path);

		MethodBase GetMethod(MethodBase method_base);

		void AddAssembly(AssemblyFile assembly_file);

		void AddAssembly(Assembly assembly);
	}
}
