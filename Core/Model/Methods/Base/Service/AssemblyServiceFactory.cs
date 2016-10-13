using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Methods.CSharp.Service;

namespace Core.Model.Methods.Base.Service
{
	/// <summary>
	/// Фабрика сервисов библиотек.
	/// </summary>
	public class AssemblyServiceFactory : IAssemblyServiceFactory
	{
		private Dictionary<Type, IAssemblyService> _assemblyServices;

		public AssemblyServiceFactory(IAssemblyService _c_sharp_assembly_service)
		{
			_assemblyServices = new Dictionary<Type, IAssemblyService>
			{
				{typeof (CSharpMethod), _c_sharp_assembly_service}
			};
		}
		
		public IAssemblyService GetAssemblyService(MethodBase method_base)
		{
			var method_type = method_base.MethodType;
			
			if (_assemblyServices.ContainsKey(method_type))
			{
				return _assemblyServices[method_type];
			}

			throw new Exception(string.Format("Тип {0} недопустим.", method_type));
		}
	}
}
