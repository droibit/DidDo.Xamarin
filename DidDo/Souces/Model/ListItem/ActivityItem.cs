using System;
using SQLite;
using System.Collections.Generic;

namespace Com.Droibit.DidDo.Models
{
	/// <summary>
	/// Activity list item.
	/// </summary>
	public class ActivityItem
	{
		#region Inner Class

		private class Comparer : EqualityComparer<ActivityItem> 
		{
			#region EqualityComparer

			public override int GetHashCode (ActivityItem item)
			{
				return item.Name.GetHashCode();
			}

			public override bool Equals (ActivityItem lhs, ActivityItem rhs)
			{
				return lhs.Name == rhs.Name;
			}

			#endregion
		}

		#endregion

		#region Public Fields

		#region Static 

		public static readonly IEqualityComparer<ActivityItem> EqualityComparer;

		#endregion

		[PrimaryKey, AutoIncrement, Unique]
		public int Id {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public DateTime RecentlyDate {
			get;
			set;
		}

		#endregion

		#region Constructor

		static ActivityItem()
		{
			EqualityComparer = new ActivityItem.Comparer ();
		}

		public ActivityItem ()
		{
		}

		#endregion
	}
}

