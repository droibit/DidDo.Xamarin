using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Com.Droibit.DidDo.Views
{
	/// <summary>
	/// アクティビティのリスト項目のビュー
	/// </summary>
	public class ActivityListItemView : RelativeLayout
	{
		#region Public Properties

		public TextView NameView {
			get;
			private set;
		}

		public TextView DateView {
			get;
			private set;
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Com.Droibit.DidDo.View.ActivityListItemView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public ActivityListItemView (Context context) :
			base (context)
		{
			LayoutInflater.From (context).Inflate (Resource.Layout.ListViewItemActivity, this);

			NameView = FindViewById<TextView> (Resource.Id.activity_name);
			DateView = FindViewById<TextView> (Resource.Id.activity_recently_date);
		}

		#endregion
	}
}

