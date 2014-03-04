using System;
using SQLite;

namespace Com.Droibit.DidDo.Models
{
	/// <summary>
	/// Activity date item.
	/// </summary>
	public class ActivityDateItem
	{
		#region Public Properties

		[PrimaryKey, AutoIncrement, Unique]
		public int Id {
			get;
			set;
		}

		public int ActivityId {
			get;
			set;
		}

		public DateTime Date {
			get;
			set;
		}

		public string Memo {
			get;
			set;
		}

		#endregion

		#region Constructor

		public ActivityDateItem ()
		{
		}

		#endregion
	}
}

