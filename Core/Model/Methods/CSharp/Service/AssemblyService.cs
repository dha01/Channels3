using System;
using System.Reflection;
using Core.Model.Methods.CSharp.DomainModel;

namespace Core.Model.Methods.CSharp.Service
{
	public class AssemblyService : IAssemblyService
	{
		public Assembly GetAssemblyForMethod(CSharpMethod csharp_method)
		{
			throw new NotImplementedException();
			/*	Assembly assembly = null;
			
			if (!AssamblyService.TryGetAssembly(string.Format("{0}", Path), out assembly))
			{
				assembly = Assembly.LoadFrom(Path);
				AssamblyService.AddAssembly(Path, assembly);
			}
			
			return assembly.GetType(csharp_method.TypeName);
			var m = t.GetMethod(csharp_method.MethodName);
			var obj = Activator.CreateInstance(t);*/
		}
	}
}
