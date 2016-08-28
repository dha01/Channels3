using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model.Network.DataModel;

namespace Core.Model.Data.DataModel
{
	public class DataBase
	{
		public Guid Id { get; set; }
		public Guid MethodId { get; set; }
		public Guid[] InputIds { get; set; }
		private object _value { get; set; }

		public Node Sender { get; set; }
		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				HasValue = true;
			}
		}

		public bool HasValue { get; set; }

		public DataState DataState { get; set; }

		public DataBase(Guid id, object value)
		{
			Id = id;
			Value = value;
			DataState = DataState.Complite;
		}

		public DataBase(Guid id)
		{
			Id = id;
			DataState = DataState.Unknown;
		}

		public DataBase() : this(Guid.NewGuid())
		{

		}
		public DataBase(object value)
			: this(Guid.NewGuid(), value)
		{

		}
	}
}
