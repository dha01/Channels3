using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Invoke.Base.Service;

namespace Core.Model.Methods.Base.DomainModel
{
	public abstract class MethodBase
	{
		public virtual Type InvokeServiceType
		{
			get { return typeof (InvokeServiceBase); } 
		}
		
		public Guid Id { get; set; }
	}
}
