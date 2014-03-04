using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Droibit.DidDo.Fragments;
using Com.Droibit.DidDo.Models;

namespace Com.Droibit.DidDo
{
	/// <summary>
	/// Master activity.
	/// </summary>
	public class MasterActivity : Activity, MasterFragment.Callbacks
	{
		#region Private Fields

		private bool mTwoPane = false;

		#endregion

		#region Protected Methods

		#region Activity

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.ActivityMasterContent);

			if (FindViewById<View> (Resource.Id.activity_detail_container) != null) {
				var f = FragmentManager.FindFragmentById (Resource.Id.activity_list) as MasterFragment;
				mTwoPane = true;
				f.ActivateOnItemClick = true;
			}

			ActionBar.SetTitle (Resource.String.app_activity);

			SQLiteManager.Instance.Open (Assets);
		}

		#endregion

		#region Callbacks

		public void OnClickdListItem(ActivityItem item)
		{
			if (mTwoPane) {
				var df = new DetailFragment ();
				FragmentManager.BeginTransaction ()
					.Replace (Resource.Id.activity_detail_container, df)
					.AddToBackStack(null)
					.Commit ();
				return;
			}
			var detailIntent = new Intent (this, typeof(DetailActivity));
			detailIntent.PutExtra (DetailFragment.ArgActivityId, item.Id)
				.PutExtra (DetailFragment.ArgActivityName, item.Name);
			StartActivity (detailIntent);
		}

		#endregion

		#endregion
	}
}


