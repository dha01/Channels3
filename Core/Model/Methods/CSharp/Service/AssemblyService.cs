using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Model.Invoke.Local.CSharp.Service;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.DomainModel;

namespace Core.Model.Methods.CSharp.Service
{
	public class AssemblyService : IAssemblyService
	{
		private Dictionary<string, AssemblyFile> _assemblyFiles;
		private Dictionary<string, Assembly> _assemblies;

		public AssemblyService()
		{
			_assemblies = new Dictionary<string, Assembly>();
			_assemblyFiles = new Dictionary<string, AssemblyFile>();
		}

		public AssemblyFile GetAssemblyFile(string path)
		{
			return _assemblyFiles.ContainsKey(path) ? _assemblyFiles[path] : null;
		}

		public Assembly GetAssembly(string path)
		{
			return _assemblies.ContainsKey(path) ? _assemblies[path] : null;
		}

		public Base.DomainModel.MethodBase GetMethod(Base.DomainModel.MethodBase method_base)
		{
			var assembly = GetAssembly(method_base.AssemblyPath);
			if (assembly == null)
			{
				throw new Exception("Не найдено библиотеки.");
			}
			var types = assembly.GetTypes();
			var type = assembly.GetType(method_base.TypeName);
			var d = Type.GetType("System.Double");
			var ds = Type.GetType("double");
			var x = method_base.InputParamsTypeNames.Select(Type.GetType).ToArray();
			var method_info = type.GetMethod(method_base.MethodName, x);

			return new CSharpMethod()
			{
				Type = type,
				MethodInfo = method_info,
				Version = method_base.Version,
				InputParamsTypeNames = method_base.InputParamsTypeNames,
				TypeName = method_base.TypeName,
				MethodName = method_base.MethodName,
				Namespace = method_base.Namespace
			};
		}

		public void AddAssembly(AssemblyFile assembly_file)
		{
			if (_assemblyFiles.ContainsKey(assembly_file.AssemblyPath))
			{
				return;
			}
			
			_assemblyFiles.Add(assembly_file.AssemblyPath, assembly_file);

			if (_assemblies.ContainsKey(assembly_file.AssemblyPath))
			{
				return;
			}
			
			var assembly = Assembly.Load(assembly_file.Data);

			_assemblies.Add(assembly_file.AssemblyPath, assembly);
		}

		public void AddAssembly(Assembly assembly)
		{
			var assembly_file = new AssemblyFile
			{
				Data = File.ReadAllBytes(assembly.Location),
				Version = assembly.GetName().Version.ToString(),
				Namespace = assembly.GetName().Name
			};
			
			if (_assemblies.ContainsKey(assembly_file.AssemblyPath))
			{
				return;
			}

			_assemblies.Add(assembly_file.AssemblyPath, assembly);

			if (_assemblyFiles.ContainsKey(assembly_file.AssemblyPath))
			{
				return;
			}

			AddAssembly(assembly_file);
		}
	}
}
