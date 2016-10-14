using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Methods.Base.Service;

namespace Core.Model.Methods.CSharp.Service
{
	public interface ICSharpAssemblyService : IAssemblyService
	{
		Assembly GetAssembly(string path);

		void AddAssembly(Assembly assembly);
	}
}
