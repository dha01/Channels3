using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Model.Invoke.Local.CSharp.Service;
using Core.Model.Methods.CSharp.DomainModel;

namespace Core.Model.Methods.CSharp.Service
{
	public class AssemblyService : IAssemblyService
	{
		private Dictionary<Guid, Assembly> _assemblies;

		private Dictionary<Assembly, List<CSharpMethod>> _assemblieMethods;

		private Dictionary<Type, List<CSharpMethod>> _typeMethods;

		public AssemblyService()
		{
			_assemblies = new Dictionary<Guid, Assembly>();
			_assemblieMethods = new Dictionary<Assembly, List<CSharpMethod>>();
			_typeMethods = new Dictionary<Type, List<CSharpMethod>>();
		}

		public List<CSharpMethod> GetMethods(Type type)
		{
			return _typeMethods[type];
		}

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

		public Assembly GetAssemblyById(Guid guid)
		{
			throw new NotImplementedException();
		}
		/*
		public Guid AddAssembly(byte[] assembly_bytes)
		{
			var guid = Guid.NewGuid();

			AddAssembly(guid, Assembly.Load(assembly_bytes));

			return guid;
		}

		public Guid AddAssembly(Type type)
		{
			var guid = Guid.NewGuid();
			AddAssembly(guid, type.Assembly);
			return guid;
		}
		public void AddAssembly(Guid guid, byte[] assembly_bytes)
		{
			AddAssembly(guid, Assembly.Load(assembly_bytes));
		}

		public void AddAssembly(Guid guid, Assembly assembly)
		{
			_assemblies.Add(guid, assembly);

			foreach (var type in assembly.GetTypes())
			{
				AddType(guid, type);
			}
		}*/

		private void AddType(Guid assembly_guid, Type type)
		{
			foreach (var method in type.GetMethods())
			{
				AddMethod(method);
			}
		}

		public Guid AddMethod(MethodInfo method)
		{
			var csharp_method = new CSharpMethod()
			{
				Id = Guid.Empty,//Guid.NewGuid(),
				TypeName = method.GetType().FullName,
				MethodName = method.Name,
				MethodInfo = method
			};

			var type = method.ReflectedType;
			var assembly = type.Assembly;

			if (!_assemblies.Values.Contains(assembly))
			{
				_assemblies.Add(Guid.NewGuid(), assembly);
			}

			if (!_assemblieMethods.ContainsKey(assembly))
			{
				_assemblieMethods.Add(assembly, new List<CSharpMethod>());
			}

			_assemblieMethods[assembly].Add(csharp_method);

			if (!_typeMethods.ContainsKey(type))
			{
				_typeMethods.Add(type, new List<CSharpMethod>());
			}

			_typeMethods[type].Add(csharp_method);

			return csharp_method.Id;
		}
	}
}
