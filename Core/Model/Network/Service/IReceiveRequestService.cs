﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Network.DataModel;

namespace Core.Model.Network.Service
{
	public interface IReceiveRequestService<T>
	{
		Action<T> OnReceive { get; set; }
	}
}
