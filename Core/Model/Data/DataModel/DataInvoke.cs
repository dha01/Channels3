using System;
using Core.Model.Invoke.Base.DataModel;

namespace Core.Model.Data.DataModel
{
	/// <summary>
	/// Исполняемые данные.
	/// </summary>
	public class DataInvoke : DataBase
	{
		#region Properties

		/// <summary>
		/// Данные запрашиваются.
		/// </summary>
		public bool IsRequestData { get; set; }
		
		/// <summary>
		/// Тип исполнения.
		/// </summary>
		public InvokeType? InvokeType { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Инициализирует по идентификатору и значению.
		/// </summary>
		/// <param name="id">Идентфиикатор.</param>
		/// <param name="value">Значение.</param>
		public DataInvoke(Guid id, object value)
			: base(id, value)
		{
		}

		/// <summary>
		/// Инициализирует по идентификатору с пустым начением.
		/// </summary>
		/// <param name="id">Идентфиикатор.</param>
		public DataInvoke(Guid id)
			: base(id)
		{
		}

		/// <summary>
		/// Инициализирует с новым идентификатором и пустым значением.
		/// </summary>
		public DataInvoke() 
			: this(Guid.NewGuid())
		{
		}

		/// <summary>
		/// Инициализирует с указанным значением и новым идентфиикатором.
		/// </summary>
		/// <param name="value">Значение.</param>
		public DataInvoke(object value)
			: this(Guid.NewGuid(), value)
		{
		}

		#endregion
	}
}