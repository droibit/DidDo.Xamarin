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
	/// Activity date list adapter.
	/// </summary>
	public class ActivityDateListAdapter : ArrayAdapter<ActivityDateItem>, IEnumerable<ActivityDateItem>
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Com.Droibit.DidDo.Controllers.DetailListAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public ActivityDateListAdapter(Context context) :
		base(context, Resource.Layout.ListViewItemActivityDate)
		{
		}

		#endregion

		#region Public Methods

		#region ArrayAdapter

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var itemView = convertView as DetailListItemView;
			if (itemView == null) {
				itemView = new DetailListItemView (Context);
			}

			var item = GetItem (position);
			itemView.DateView.Text = item.Date.ToString ("yy/MM/dd(ddd) HH:mm");
			itemView.MemoView.Text = item.Memo;

			return itemView;
		}

		#endregion

		#region IEnumrator

		public IEnumerator<ActivityDateItem> GetEnumerator()
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

