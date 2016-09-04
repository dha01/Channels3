using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Model.Methods.Base.DomainModel
{
	public class AssemblyFile : AssemblyInfo
	{
		public byte[] Data { get; set; }
	}
}
