using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Network.Base.DataModel
{
	public class Request
	{
		public RequestType RequestType { get; set; }
		public NodeInfo Sender { get; set; }
		public object Data { get; set; }
	}
}
