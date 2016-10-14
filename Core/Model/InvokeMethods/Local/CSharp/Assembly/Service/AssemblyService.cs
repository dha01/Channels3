using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.DomainModel;

namespace Core.Model.Methods.CSharp.Service
{
	/// <summary>
	/// Сервис для работы с библиотеками C#.
	/// </summary>
	public class AssemblyService : IAssemblyService
	{

		private readonly Dictionary<string, AssemblyFile> _assemblyFiles;
		private readonly Dictionary<string, Assembly> _assemblies;

		public AssemblyService()
		{
			_assemblies = new Dictionary<string, Assembly>();
			_assemblyFiles = new Dictionary<string, AssemblyFile>();
			LoadDefaultAssemblies();
		}

		public AssemblyService(IEnumerable<string> paths) 
			: this()
		{
			foreach (var path in paths)
			{
				AddAssembly(path);
			}
		}

		private void LoadDefaultAssemblies()
		{
			string path = System.Configuration.ConfigurationManager.AppSettings["DefaultAssembliesCSharpPath"];
			LoadAllAssembliesFromPath(path);
		}

		private void LoadAllAssembliesFromPath(string path)
		{
			var dir = new DirectoryInfo(path);
			var x = dir.FullName;
			foreach (var item in dir.GetDirectories())
			{
				LoadAllAssembliesFromPath(item.FullName);
			}

			foreach (var item in dir.GetFiles("*.dll"))
			{
				var fn = item.FullName;
				AddAssembly(item.FullName);
			}
		}

		public AssemblyFile GetAssemblyFile(string full_assembly_name)
		{
			return _assemblyFiles.ContainsKey(full_assembly_name) ? _assemblyFiles[full_assembly_name] : null;
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

			var type = assembly.GetType(method_base.TypeName);
			var input_param_types = method_base.InputParamsTypeNames.Select(Type.GetType).ToArray();
			var method_info = type.GetMethod(method_base.MethodName, input_param_types);

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

		public void AddAssembly(byte[] bytes)
		{
			var assembly = Assembly.Load(bytes);

			var assembly_file = new AssemblyFile
			{
				Data = bytes,
				Version = assembly.GetName().Version.ToString(),
				Namespace = assembly.GetName().Name
			};

			AddAssembly(assembly_file);
		}

		public void AddAssembly(string path)
		{
			AddAssembly(File.ReadAllBytes(path));
		}

		public void AddAssembly(Assembly assembly)
		{
			AddAssembly(assembly.Location);
		}
	}
}
