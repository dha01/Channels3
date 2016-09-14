using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Model.Network.DataModel;

namespace Core.Model.Network.Base.Service
{
	[WebClass(Namespace = NAMESPACE)]
	public class NotificationService : INotificationService
	{
		private const string NAMESPACE = "Notification";
		
		private IUdpServerService _udpServerService;

		private Action<object> _onReceiveNotify = (obj)=>{};

		public NotificationService()
		{
			_udpServerService = new UdpServerService();
		}

		public NotificationService(IUdpServerService udp_server_service)
		{
			_udpServerService = udp_server_service;
			_udpServerService.InitWebMethods(this);
		}

		public void AddAction(Action<object> action)
		{
			_onReceiveNotify += action;
		}

		[WebMethod]
		public void Notify(object value)
		{
			Console.WriteLine("Принято широковещательное уведомление: {0}", value);
			_onReceiveNotify.Invoke(value);
		}

		public void RunRegularNotify(object value, int timeout)
		{
			Task.Run(() =>
			{
				while (true)
				{
					Console.WriteLine("Отправлено широковещательное уведомление: {0}", value);
					Notify(_udpServerService, value);
					Thread.Sleep(timeout);
				}
			});
		}

		public static void Notify(IUdpServerService udp_server_service, object value)
		{
			if (udp_server_service == null)
			{
				udp_server_service = new UdpServerService();
			}
			udp_server_service.BroadcastRequest(string.Format("/{0}/Notify", NAMESPACE), value);
		}
	}
}
