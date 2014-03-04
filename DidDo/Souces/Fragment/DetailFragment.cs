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
using Com.Droibit.DidDo.Views;
using Com.Droibit.DidDo.Models;
using Com.Droibit.DidDo.Utils;

namespace Com.Droibit.DidDo.Fragments
{
	/// <summary>
	/// Detail activity fragment.
	/// </summary>
	public class DetailFragment : Fragment, AddActivityDateDialogFragment.Callbacks,
		ModifyTextDialogFragment.Callbacks
	{
		#region Private Fields

		private DetailActivityViewHolder mViewHolder;

		private SQLiteManager mSqlite;

		private int mActivityId;

		#endregion

		#region Public Fields

		public static readonly string ArgActivityId = "ActivityId";

		public static readonly string ArgActivityName = "ActivityName";

		#endregion

		#region Public Methods

		public static DetailFragment NewInstance(int activityId)
		{
			var args = new Bundle (1);
			args.PutInt (ArgActivityId, activityId);

			var f = new DetailFragment ();
			f.Arguments = args;

			return f;
		}

		#region Fragment

		/// <Docs>The options menu in which you place your items.</Docs>
		/// <summary>
		/// Initialize the contents of the Activity's standard options menu.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/// <param name="inflater">Inflater.</param>
		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu (menu, inflater);

			inflater.Inflate (Resource.Menu.DetailMenu, menu);
		}

		/// <Docs>The menu item that was selected.</Docs>
		/// <returns>To be added.</returns>
		/// <para tool="javadoc-to-mdoc">This hook is called whenever an item in your options menu is selected.
		///  The default implementation simply returns false to have the normal
		///  processing happen (calling the item's Runnable or sending a message to
		///  its Handler as appropriate). You can use this method for any items
		///  for which you would like to do processing without those other
		///  facilities.</para>
		/// <summary>
		/// Raises the options item selected event.
		/// </summary>
		/// <param name="item">Item.</param>
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				Activity.NavigateUpTo(new Intent(Activity, typeof(MasterActivity)));
				return true;
			case Resource.Id.action_new_activity_date:
				var dialog = new AddActivityDateDialogFragment ();
				dialog.Show (this);
				break;
			}
			return base.OnOptionsItemSelected (item);
		}

		/// <Docs>The context menu that is being built</Docs>
		/// <param name="menuInfo">Extra information about the item for which the
		///  context menu should be shown. This information will vary
		///  depending on the class of v.</param>
		/// <c>view</c>
		/// <summary>
		/// Raises the create context menu event.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/// <param name="v">V.</param>
		public override void OnCreateContextMenu (IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
		{
			base.OnCreateContextMenu (menu, v, menuInfo);

			//menu.SetHeaderTitle (Resource.String.context_menu_header);
			menu.Add (Menu.None, Resource.Id.action_modify_activity_memo, Menu.None, 
				Resource.String.action_title_modify_activity_memo);
			menu.Add (Menu.None, Resource.Id.action_delete_activity_date, Menu.None, 
				Resource.String.action_title_delete_activity_date);
		}

		/// <Docs>The context menu item that was selected.</Docs>
		/// <returns>To be added.</returns>
		/// <para tool="javadoc-to-mdoc">This hook is called whenever an item in a context menu is selected. The
		///  default implementation simply returns false to have the normal processing
		///  happen (calling the item's Runnable or sending a message to its Handler
		///  as appropriate). You can use this method for any items for which you
		///  would like to do processing without those other facilities.</para>
		/// <summary>
		/// Raises the context item selected event.
		/// </summary>
		/// <param name="item">Item.</param>
		public override bool OnContextItemSelected (IMenuItem item)
		{
			var menuInfo = item.MenuInfo as AdapterView.AdapterContextMenuInfo;

			switch (item.ItemId) {
			case Resource.Id.action_modify_activity_memo:
				{
					var activityDate = mViewHolder.ListAdapter.GetItem (menuInfo.Position);
					var info = new ModifyTextDialogFragment.Info {
						Text = activityDate.Memo,
						TargetPosition = menuInfo.Position,
						ViewId = Resource.Layout.DialogAddActivityDate,
						DialogTitleId = Resource.String.dialog_title_modify_activity_memo,
					};
					var dialog = ModifyTextDialogFragment.NewInstance (info);
					dialog.Show (this);
				}
				return true;
			case Resource.Id.action_delete_activity_date:
				DeleteActivityDate(mViewHolder.ListAdapter.GetItem(menuInfo.Position));
				return true;
			}
			return base.OnContextItemSelected (item);
		}

		/// <Docs>If the fragment is being re-created from
		///  a previous saved state, this is the state.</Docs>
		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			if (Arguments.ContainsKey (ArgActivityId)) {
				mActivityId = Arguments.GetInt (ArgActivityId);
			}
			mSqlite = SQLiteManager.Instance;
		}

		/// <Docs>The LayoutInflater object that can be used to inflate
		///  any views in the fragment,</Docs>
		/// <param name="savedInstanceState">If non-null, this fragment is being re-constructed
		///  from a previous saved state as given here.</param>
		/// <returns>To be added.</returns>
		/// <summary>
		/// Raises the create view event.
		/// </summary>
		/// <param name="inflater">Inflater.</param>
		/// <param name="container">Container.</param>
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			mViewHolder = new DetailActivityViewHolder (Activity, container);
			RegisterForContextMenu (mViewHolder.ListView);
			return mViewHolder.View;
		}

		/// <Docs>If the fragment is being re-created from
		///  a previous saved state, this is the state.</Docs>
		/// <summary>
		/// Raises the activity created event.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);

			SetHasOptionsMenu (true);
			RetainInstance = true;

			RefleshListContents ();
		}

		public override void OnDetach ()
		{
			base.OnDetach ();
		}

		#endregion

		#region AddActivityDateDialogFragment.Callbacks

		public async void OnEnteredActivityDate(string memo)
		{
			var item = new ActivityDateItem () {
				ActivityId = mActivityId,
				Memo = memo,
				Date = DateTime.Now
			};

			var resItem = await mSqlite.AddActivityDateAsync(item);
			if (resItem != null) {
				mViewHolder.ListAdapter.Insert(resItem, 0);
				UpdateElapsedDate ();
				ToastManager.ShowLongTime (Activity, Resource.String.toast_success_added_activity_date);
			} else {
				ToastManager.ShowLongTime (Activity, Resource.String.toast_failed_added_activity_date);
			}
		}

		#endregion

		#region ModifyTextDialogFragment.Callbacks

		public async void OnModifiedText(int targetPosition, string modifiedText)
		{
			modifiedText = String.IsNullOrEmpty(modifiedText) ? "---" : modifiedText;
			var activityDate = mViewHolder.ListAdapter.GetItem (targetPosition);
			activityDate.Memo = modifiedText;

			if (await mSqlite.UpdateActivityDateAsync (activityDate) > 0) {
				mViewHolder.ListAdapter.NotifyDataSetChanged ();
				ToastManager.ShowLongTime (Activity, Resource.String.toast_success_modify_activity_memo);
			} else {
				ToastManager.ShowLongTime (Activity, Resource.String.toast_failed_modify_activity_memo);
			}
		}

		#endregion

		#endregion

		#region Privaet Methods

		private async void RefleshListContents() 
		{
			var dateList = await mSqlite.LoadActivityDateListAsync (mActivityId);
			if (dateList.Count == 0) {
				return;
			}

			mViewHolder.ListAdapter.Clear ();
			foreach (var date in dateList) {
				mViewHolder.ListAdapter.Add (date);
			}

			UpdateElapsedDate ();
		}

		private async void DeleteActivityDate(ActivityDateItem item)
		{
			int resCount = await mSqlite.DeleteActivityDateAsync(item);
			if (resCount > 0) {
				mViewHolder.ListAdapter.Remove (item);
				UpdateElapsedDate ();
				ToastManager.ShowLongTime (Activity, Resource.String.toast_success_delete_activity_date);
			} else {
				ToastManager.ShowLongTime (Activity, Resource.String.toast_failed_delete_activity_date);
			}
		}

		private async void UpdateElapsedDate()
		{
			if (mViewHolder.ListAdapter.Count == 0) {
				mViewHolder.ElapsedDateView.Text = GetString (Resource.String.format_elapsed_date, 0);
				return;
			}

			var recentlyActivityDate = await mSqlite.GetRecentlyActivityDateAsync (mActivityId);
			var span = DateTime.Now - recentlyActivityDate;

			mViewHolder.ElapsedDateView.Text = GetString (Resource.String.format_elapsed_date, span.Days);
		}

		#endregion
	}
}

