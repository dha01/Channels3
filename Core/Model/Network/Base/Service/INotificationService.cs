using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Network.Base.Service
{
	public interface INotificationService
	{
		void AddAction(Action<object> action);

		void RunRegularNotify(object value, int timeout);
	}
}
