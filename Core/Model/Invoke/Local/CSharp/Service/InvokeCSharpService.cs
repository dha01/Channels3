using System;
using System.Linq;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
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
		private IDataService<DataInvoke> _dataService; 
		
		protected override InvokeType InvokeType
		{
			get
			{
				return InvokeType.Local;
			}
		}

		public InvokeCSharpService()
			:this(new AssemblyService(), new MethodService(), new DataService<DataInvoke>())
		{
			
		}

		public InvokeCSharpService(IAssemblyService assembly_service, IMethodService method_service, IDataService<DataInvoke> data_service)
		{
			_assemblyService = assembly_service;
			_methodService = method_service;
			_dataService = data_service;
		}

		protected override void InvokeMethod(DataInvoke invoked_data, Action<DataInvoke> callback)
		{
			var method = (CSharpMethod)_methodService.GetMethod(invoked_data.MethodId);
			//var assembly = _assemblyService.GetAssemblyForMethod(method);

			var t = method.MethodInfo.ReflectedType;// assembly.GetType(method.TypeName);
			var m = method.MethodInfo;
			var obj = Activator.CreateInstance(t);

			try
			{
				var inputs = invoked_data.InputIds.Select(x => _dataService.Get(x).Value).ToArray();
				invoked_data.Value = m.Invoke(obj, inputs);
			}
			catch (Exception e)
			{
				invoked_data.Value = e.InnerException;
			}
			callback.Invoke(invoked_data);
		}
	}
}
