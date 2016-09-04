using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.Service;

namespace Core.Model.Methods.Base.Service
{
	public interface IAssemblyServiceFactory
	{
		IAssemblyService GetAssemblyService(MethodBase method_base);

	}
}
