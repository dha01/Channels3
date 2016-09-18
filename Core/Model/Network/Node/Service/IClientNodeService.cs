using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;

namespace Core.Model.Network.Node.Service
{
	public interface IClientNodeService
	{
		void Invoke(DataInvoke data_invoke);
		object Get(Guid guid);

	}
}
