using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Data.DataModel;
using Core.Model.Invoke.Base.DataModel;
using Core.Model.Invoke.Local.CSharp.Service;
using Core.Model.Invoke.Remote.Service;
using Core.Model.Methods.Base.Service;

namespace Core.Model.Invoke.Base.Service
{
	public class InvokeServiceFactory : IInvokeServiceFactory
	{

		private IMethodService _methodService;
		
		private readonly Dictionary<Type, IInvokeService> _serviceDictionary;

		public InvokeServiceFactory() 
			: this(new MethodService())
		{
		}

		public void AddOnDequeueEvent(Action<DataInvoke> action)
		{
			foreach (var service in _serviceDictionary)
			{
				service.Value.OnAfterInvoke += action;
			}
		}

		public InvokeServiceFactory(IMethodService method_service)
		{
			_methodService = method_service;
			_serviceDictionary = new Dictionary<Type, IInvokeService>
			{
				{typeof(RemoteInvokeService), new RemoteInvokeService()},
				{typeof(InvokeCSharpService), new InvokeCSharpService()}
			};
		}
		
		public IInvokeService GetInvokeService(DataInvoke invoked_data)
		{
			switch (invoked_data.InvokeType)
			{
				case InvokeType.Remote:
					return _serviceDictionary[typeof(RemoteInvokeService)];
				case InvokeType.Local:
					return _serviceDictionary[_methodService.GetMethod(invoked_data.MethodId).InvokeServiceType];
			}
			throw new Exception(string.Format("Тип {0} недопустим.", invoked_data.InvokeType));
		}
	}
}
