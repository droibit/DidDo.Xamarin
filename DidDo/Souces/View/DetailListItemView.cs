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
	/// アクティビティの詳細項目のビュー
	/// </summary>
	public class DetailListItemView : RelativeLayout
	{
		#region Public Properties

		public TextView DateView {
			get;
			private set;
		}

		public TextView MemoView {
			get;
			private set;
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Com.Droibit.DidDo.Views.DetailListItemView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public DetailListItemView (Context context) :
			base(context)
		{
			LayoutInflater.From (context).Inflate (Resource.Layout.ListViewItemActivityDate, this);

			DateView = FindViewById<TextView> (Resource.Id.activity_date);
			MemoView = FindViewById<TextView> (Resource.Id.activity_memo);
		}

		#endregion
	}
}

