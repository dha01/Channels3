using Core.Model.Methods.Base.DomainModel;

namespace Core.Model.Methods.Base.Service
{
	public interface IAssemblyService
	{
		AssemblyFile GetAssemblyFile(string full_assembly_name);

		MethodBase GetMethod(MethodBase method_base);

		void AddAssembly(AssemblyFile assembly_file);

		void AddAssembly(string path);
	}
}
