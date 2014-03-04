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
using Com.Droibit.DidDo.Controllers;
using Com.Droibit.DidDo.Models;
using Com.Droibit.DidDo.Utils;
using Android.Preferences;

namespace Com.Droibit.DidDo.Fragments
{
	/// <summary>
	/// 活動をリスト表示するためのフラグメント
	/// </summary>
	public class MasterFragment : ListFragment, AddActivityDialogFragment.Callbacks,
		SortActivityDialogFragment.Callbacks, 
		ModifyTextDialogFragment.Callbacks
	{
		#region Inner Interface

		public interface Callbacks 
		{
			void OnClickdListItem(ActivityItem item);
		}

		#endregion

		#region Private Fields

		#region Static Fields

		private static readonly string StateActivatedPosition = "ActivatedPosition";

		private static readonly string ArgSelectedActivityId = "SelectedActivityId";

		private static readonly string PrefsSortItemPosition = "SortItemPosition";

		#endregion

		private SQLiteManager mSqlite;

		private ActivityListAdapter mListAdapter;

		private Callbacks mCallbacks;

		private int mActivatedPosition = ListView.InvalidPosition;

		#endregion

		#region Public Properties

		public bool ActivateOnItemClick
		{
			set {
				ListView.ChoiceMode = value ? ChoiceMode.Single
					: ChoiceMode.None;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Com.Droibit.DidDo.Fragment.ActivityListFragment"/> class.
		/// </summary>
		public MasterFragment()
		{
			Arguments = new Bundle (1);
		}

		#endregion

		#region Public Methods

		#region ListFragment

		/// <Docs>The options menu in which you place your items.</Docs>
		/// <summary>
		/// Initialize the contents of the Activity's standard options menu.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/// <param name="inflater">Inflater.</param>
		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu (menu, inflater);

			inflater.Inflate (Resource.Menu.MasterMenu, menu);
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
			case Resource.Id.action_new_activity:
				{
					var dialog = new AddActivityDialogFragment ();
					dialog.Show (this);
				}
				return true;
			case Resource.Id.action_sort:
				{
					int positin = PreferenceManager.GetDefaultSharedPreferences (Activity)
						.GetInt (PrefsSortItemPosition, 0);
					var dialog = SortActivityDialogFragment.NewInstance (positin);
					dialog.Show (this);
				} 
				return true;
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
			//menu.Add (Menu.None, Resource.Id.action_modify_activity, Menu.None, Resource.String.action_title_modify_activity);
			menu.Add (Menu.None, Resource.Id.action_delete_activity, Menu.None, Resource.String.action_title_delete_activity);	
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
			case Resource.Id.action_modify_activity:
				{
					var activity = mListAdapter.GetItem (menuInfo.Position);
					var info = new ModifyTextDialogFragment.Info {
						Text = activity.Name,
						TargetPosition = menuInfo.Position,
						ViewId =  Resource.Layout.DialogAddActivity,
						DialogTitleId = Resource.String.dialog_title_modify_activity,
					};
					var dialog = ModifyTextDialogFragment.NewInstance (info);
					dialog.Show (this);
				}
				return true;
			case Resource.Id.action_delete_activity:
				DeleteActivity(mListAdapter.GetItem(menuInfo.Position));
				return true;
			}
			return base.OnContextItemSelected (item);
		}

		/// <Docs>To be added.</Docs>
		/// <summary>
		/// Raises the attach event.
		/// </summary>
		/// <param name="activity">Activity.</param>
		public override void OnAttach (Activity activity)
		{
			base.OnAttach (activity);

			mCallbacks = activity as Callbacks;
			if (mCallbacks == null) {
				throw new InvalidCastException ("Activity must implement fragment's callbacks.");
			}
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

			mSqlite = SQLiteManager.Instance;
		}

		/// <summary>
		/// Raises the view created event.
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="savedInstanceState">Saved instance state.</param>
		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			ListAdapter = mListAdapter = new ActivityListAdapter (Activity); 

			if (savedInstanceState != null
				&& savedInstanceState.ContainsKey(StateActivatedPosition)) {
				SetActivatedPosition(savedInstanceState.GetInt(StateActivatedPosition));
			}

			RegisterForContextMenu (ListView);
			ListView.FastScrollEnabled = true;
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

			SetEmptyText (GetString(Resource.String.empty_activity_list));
			SetHasOptionsMenu (true);
			RetainInstance = true;
		
			RefleshListContents ();
		}

		/// <Docs>Called when the fragment is visible to the user and actively running.</Docs>
		/// <summary>
		/// Raises the resume event.
		/// </summary>
		public override void OnResume ()
		{
			if (Arguments.ContainsKey(ArgSelectedActivityId)) {
				RefleshPartOfListContents (Arguments.GetInt(ArgSelectedActivityId));
				Arguments.Remove(ArgSelectedActivityId);
			}
			base.OnResume ();
		}

		/// <Docs>The ListView where the click happened</Docs>
		/// <param name="position">The position of the view in the list</param>
		/// <summary>
		/// This method will be called when an item in the list is selected.
		/// </summary>
		/// <para tool="javadoc-to-mdoc">This method will be called when an item in the list is selected.
		///  Subclasses should override. Subclasses can call
		///  getListView().getItemAtPosition(position) if they need to access the
		///  data associated with the selected item.</para>
		/// <format type="text/html">[Android Documentation]</format>
		/// <since version="Added in API level 11"></since>
		/// <param name="l">L.</param>
		/// <param name="v">V.</param>
		/// <param name="id">Identifier.</param>
		public override void OnListItemClick (ListView l, View v, int position, long id)
		{
			base.OnListItemClick (l, v, position, id);

			var selectedItem = mListAdapter.GetItem (position);
			mCallbacks.OnClickdListItem (selectedItem);

			Arguments.PutInt (ArgSelectedActivityId, selectedItem.Id);
		}

		/// <Docs>Bundle in which to place your saved state.</Docs>
		/// <summary>
		/// Raises the save instance state event.
		/// </summary>
		/// <param name="outState">Out state.</param>
		public override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);

			if (mActivatedPosition != ListView.InvalidPosition) {
				// Serialize and persist the activated item position.
				outState.PutInt(StateActivatedPosition, mActivatedPosition);
			}
		}

		#endregion

		#region AddActivityDialogFragment.Callbacks

		public bool IsNewActivityName(string activityName)
		{
			var item = new ActivityItem() { Name = activityName };
			return !mListAdapter.Contains (item, ActivityItem.EqualityComparer);
		}

		public async void OnEnteredNewActivity(string activityName)
		{
			var item = new ActivityItem() { Name = activityName, RecentlyDate = DateTime.Now };
			var resItem = await mSqlite.AddActivityAsync (item);
			if (resItem != null) {
				mListAdapter.Add(resItem);
				ToastManager.ShowLongTime (Activity, Resource.String.toast_success_added_activity);
			} else {
				ToastManager.ShowLongTime (Activity, Resource.String.toast_failed_add_activity);
			}
		}

		#endregion

		#region SortActivityDialogFragment.Callacks

		public void OnChoicedSortItem(int clickedPosition)
		{
			var prefs = PreferenceManager.GetDefaultSharedPreferences (Activity);
			var editor = prefs.Edit ();
			editor.PutInt (PrefsSortItemPosition, clickedPosition)
				.Commit ();

			var items = SortActivityList (mListAdapter, clickedPosition);

			mListAdapter.Clear ();
			foreach (var item in items) {
				mListAdapter.Add (item);
			}

			string name = Activity.Resources.GetTextArray(Resource.Array.sort_activity_labels)[clickedPosition];
			string text = GetString (Resource.String.format_activity_sort, name);

			ToastManager.ShowLongTime (Activity, text); 
		}

		#endregion

		#region ModifyTextDialogFragment.Callbacks

		public async void OnModifiedText(int targetPosition, string modifiedText)
		{
			var activity = mListAdapter.GetItem(targetPosition);
			if (await mSqlite.UpdateActivityAsync (activity) > 0) {
				mListAdapter.NotifyDataSetChanged ();
				ToastManager.ShowLongTime (Activity, Resource.String.toast_success_modify_activity_name);
			} else {
				ToastManager.ShowLongTime (Activity, Resource.String.toast_failed_modify_activity_name);
			}
		}

		#endregion

		#endregion

		#region Private Methods

		private async void RefleshListContents() 
		{
			SetListShown (false);

			var activities = await mSqlite.LoadActivityListAsync ();
			if (activities.Count == 0) {
				return;
			}

			var prefs = PreferenceManager.GetDefaultSharedPreferences (Activity);
			var sortedActivities = SortActivityList(activities, prefs.GetInt (PrefsSortItemPosition, 0));

			mListAdapter.Clear ();
			foreach (var activity in sortedActivities) {
				mListAdapter.Add (activity);
			}

			SetListShown(true);
		}

		private async void RefleshPartOfListContents(int activityId)
		{
			var recentlyDate = await mSqlite.GetRecentlyActivityDateAsync (activityId);
			var targetItem = mListAdapter.Where (i => i.Id == activityId).First ();
			if (targetItem != null) {
				targetItem.RecentlyDate = recentlyDate;
				mListAdapter.NotifyDataSetChanged ();
			}
		}

		private IList<ActivityItem> SortActivityList(IEnumerable<ActivityItem> activities,int sortType)
		{
			switch (sortType) {
			case 0:	// Name
				return activities.OrderBy (i => i.Name).ToList ();
			case 1:	// Recently Date
				return activities.OrderByDescending (i => i.RecentlyDate).ToList ();
			}
			return null;
		}

		private async void DeleteActivity(ActivityItem item)
		{
			int resCount = await mSqlite.DeleteActivityAsync(item);
			if (resCount > 0) {
				mListAdapter.Remove (item);
				ToastManager.ShowLongTime (Activity, Resource.String.toast_success_deleted_activity);
			} else {
				ToastManager.ShowLongTime (Activity, Resource.String.toast_failed_delete_activity);
			}
		}

		private void SetActivatedPosition(int position)
		{
			if (position == ListView.InvalidPosition) {
				ListView.SetItemChecked(mActivatedPosition, false);
			} else {
				ListView.SetItemChecked(position, true);
			}
			mActivatedPosition = position;
		}

		#endregion
	}
}

