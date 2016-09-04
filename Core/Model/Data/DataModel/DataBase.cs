using System;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Network.DataModel;

namespace Core.Model.Data.DataModel
{
	/// <summary>
	/// Базовый объект с данными.
	/// </summary>
	public class DataBase
	{
		#region Properties

		/// <summary>
		/// Идентфиикатор.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Метод исполнения.
		/// </summary>
		public MethodBase Method { get; set; }

		/// <summary>
		/// Входные параметры.
		/// </summary>
		public Guid[] InputIds { get; set; }

		/// <summary>
		/// Отправитель.
		/// </summary>
		public Node Sender { get; set; }

		private object _value { get; set; }

		/// <summary>
		/// Значение.
		/// </summary>
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

		/// <summary>
		/// Флаг наличия значения.
		/// </summary>
		public bool HasValue { get; set; }

		/// <summary>
		/// Состояние данных.
		/// </summary>
		public DataState DataState { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Инициализирует по идентификатору и значению.
		/// </summary>
		/// <param name="id">Идентфиикатор.</param>
		/// <param name="value">Значение.</param>
		public DataBase(Guid id, object value)
		{
			Id = id;
			Value = value;
			DataState = DataState.Complite;
		}

		/// <summary>
		/// Инициализирует по идентификатору с пустым начением.
		/// </summary>
		/// <param name="id">Идентфиикатор.</param>
		public DataBase(Guid id)
		{
			Id = id;
			DataState = DataState.Unknown;
		}

		/// <summary>
		/// Инициализирует с новым идентификатором и пустым значением.
		/// </summary>
		public DataBase() : this(Guid.NewGuid())
		{

		}

		/// <summary>
		/// Инициализирует с указанным значением и новым идентфиикатором.
		/// </summary>
		/// <param name="value">Значение.</param>
		public DataBase(object value)
			: this(Guid.NewGuid(), value)
		{

		}

		#endregion
	}
}
