using System;
using System.Linq;
using System.Reflection;
using Core.Model.Data.DataModel;
using Core.Model.Data.Service;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Base.Service;
using Core.Model.Methods.Base.Service;
using Core.Model.Methods.CSharp.DomainModel;
using Core.Model.Methods.CSharp.Service;
using Core.Model.Network.DataModel;
using Core.Model.Network.Service;

namespace Core.Model.Invoke.Local.CSharp.Service
{
	public class InvokeCSharpService : InvokeServiceBase
	{
		private IAssemblyService _assemblyService;

		private IMethodService _methodService;
		private IDataService<DataInvoke> _dataService;

		//private ISendRequestService _sendRequestService;
		
		protected override InvokeType InvokeType
		{
			get
			{
				return InvokeType.Local;
			}
		}
		/*
		public InvokeCSharpService()
			:this(new AssemblyService(), new MethodService(), new DataService<DataInvoke>())
		{
			
		}*/

		public InvokeCSharpService(IAssemblyService assembly_service, IMethodService method_service, IDataService<DataInvoke> data_service)
		{
			_assemblyService = assembly_service;
			_methodService = method_service;
			_dataService = data_service;
		}

		private CSharpMethod RequestMethod(NodeInfo sender, Guid method_id)
		{
			//_sendRequestService.
			return null;
		}

		private CSharpMethod GetMethod(Methods.Base.DomainModel.MethodBase method)
		{
			var result = (CSharpMethod)_methodService.GetMethod(method);
			if (result == null)
			{
				result = (CSharpMethod)method;
				_methodService.AddMethod(method);
			}

			if (result.MethodInfo == null)
			{
				var assembly_file = _assemblyService.GetAssemblyFile(method.AssemblyPath);

				if (assembly_file == null)
				{
					throw new Exception(string.Format("InvokeCSharpService -> Библиотека не найдена: {0}", method.AssemblyPath));
				}

				var assembly = Assembly.Load(assembly_file.Data);
				var type = assembly.GetType(method.TypeName);
				result.MethodInfo = type.GetMethod(method.MethodName, method.InputParamsTypeNames.Select(Type.GetType).ToArray());
			}

			return result;
		}

		protected override void InvokeMethod(DataInvoke invoked_data, Action<DataInvoke> callback)
		{
			var method = GetMethod(invoked_data.Method);

			try
			{
				var inputs = invoked_data.InputIds.Select(x => _dataService.Get(x).Value).ToArray();
				var obj = Activator.CreateInstance(method.Type);
				invoked_data.Value = method.MethodInfo.Invoke(obj, inputs);
				Console.WriteLine("Исполнен метод {0}", invoked_data.Method.MethodName);
			}
			catch (Exception e)
			{
				invoked_data.Value = e.InnerException;
			}
			callback.Invoke(invoked_data);
		}
	}
}
