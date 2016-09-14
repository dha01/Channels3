using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;

namespace Core.Model.Network.Node.Service
{
	public interface INodeService
	{
		DataInvoke GetData(Guid guid);
		bool AddData(DataInvoke data_invoke);
	}
}
