using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Droibit.DidDo.Fragments;
using Android.Content;

namespace Com.Droibit.DidDo
{
	/// <summary>
	/// 活動の詳細を表示するためのアクティビティ
	/// </summary>
	public class DetailActivity : Activity
	{
		#region Protected Methods

		#region Activity

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="savedInstanceState">c</param>
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.ActivityDetailContent);

			ActionBar.Title = GetString(Resource.String.format_detail_activity_label_format, 
				Intent.GetStringExtra(DetailFragment.ArgActivityName));
			ActionBar.SetDisplayHomeAsUpEnabled (true);

			if (savedInstanceState == null) {
				// Create the detail fragment and add it to the activity
				// using a fragment transaction.
				var df = DetailFragment.NewInstance(Intent.GetIntExtra(DetailFragment.ArgActivityId, -1));
				FragmentManager.BeginTransaction ()
					.Add (Resource.Id.activity_detail_container, df).Commit ();
			}
		}

		#endregion

		#endregion
	}
}

