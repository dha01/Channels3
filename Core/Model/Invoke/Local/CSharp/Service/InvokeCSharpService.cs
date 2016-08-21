using System;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Methods.CSharp.Service;

namespace Core.Model.Invoke.Local.CSharp.Service
{
	public class InvokeCSharpService : InvokeServiceBase
	{
		private IAssemblyService _assemblyService;

		private IMethodService _methodService;
		
		protected override InvokeType InvokeType
		{
			get
			{
				return InvokeType.Local;
			}
		}

		public InvokeCSharpService()
			:this(new AssemblyService(), new MethodService())
		{
			
		}

		public InvokeCSharpService(IAssemblyService assembly_service, IMethodService method_service)
		{
			_assemblyService = assembly_service;
			_methodService = method_service;
		}

		protected override void InvokeMethod(DataInvokeFilled invoked_data, Action<DataInvokeFilled> callback)
		{
			var method = (CSharpMethod)_methodService.GetMethod(invoked_data.MethodId);
			var assembly = _assemblyService.GetAssemblyForMethod(method);

			var t = assembly.GetType(method.TypeName);
			var m = t.GetMethod(method.MethodName);
			var obj = Activator.CreateInstance(t);

			try
			{
				invoked_data.Value = m.Invoke(obj, invoked_data.Inputs);
			}
			catch (Exception e)
			{
				invoked_data.Value = e.InnerException;
			}
			callback.Invoke(invoked_data);
		}
	}
}
