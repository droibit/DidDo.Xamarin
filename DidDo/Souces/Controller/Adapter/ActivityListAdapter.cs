using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Droibit.DidDo.Models;
using Com.Droibit.DidDo.Views;

namespace Com.Droibit.DidDo.Controllers
{
	/// <summary>
	/// 活動を
	/// </summary>
	public class ActivityListAdapter : ArrayAdapter<ActivityItem>, IEnumerable<ActivityItem>
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Com.Droibit.DidDo.Controllers.ActivityListAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public ActivityListAdapter(Context context) :
			base(context, Resource.Layout.ListViewItemActivity)
		{
		}

		#endregion

		#region Public Methods

		#region ArrayAdapter

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var itemView = convertView as ActivityListItemView;
			if (itemView == null) {
				itemView = new ActivityListItemView (Context);
			}

			var item = GetItem (position);
			itemView.NameView.Text = item.Name;
			itemView.DateView.Text = item.RecentlyDate.ToString ("yy/MM/dd(ddd)");

			return itemView;
		}

		#endregion

		#region IEnumrator
		
		public IEnumerator<ActivityItem> GetEnumerator()
		{
			for (int i = 0; i < Count; i++) {
				yield return GetItem (i);
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#endregion
	}
}

