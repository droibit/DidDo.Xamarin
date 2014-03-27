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
using Com.Droibit.DidDo.Utils;
using com.alliance.calendar;

namespace Com.Droibit.DidDo.Fragments
{
	/// <summary>
	/// 活動日をカレンダーに表示するためのフラグメント
	/// </summary>
	public class CalendarFragment : Fragment
	{
		#region Private Fields

		private int mActivityId;

		private IList<ActivityDateItem> mDateItems;

		#endregion

		#region Public Methods

		public static CalendarFragment NewInstance(int activityId)
		{
			var args = new Bundle (1);
			args.PutInt (DetailFragment.ArgActivityId, activityId);

			var f = new CalendarFragment ();
			f.Arguments = args;

			return f;
		}

		#region Fragment

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
			}
			return base.OnOptionsItemSelected (item);
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

			Activity.ActionBar.SetTitle (Resource.String.actionbar_calendar);

			mActivityId = Arguments.GetInt (DetailFragment.ArgActivityId);
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
			var containerView = inflater.Inflate (Resource.Layout.FragmentCalendar, container, false);
			var calendarView = containerView.FindViewById<CustomCalendar> (Resource.Id.calendar);
			calendarView.NextButtonText = GetString(Resource.String.calendar_next_month);
			calendarView.PreviousButtonText = GetString (Resource.String.calendar_prev_month);
			calendarView.NextButtonStyleId = calendarView.PreviousButtonStyleId = 
				Resource.Drawable.list_selector_holo;
			calendarView.ShowFromDate = new DateTime();

			ReadContents (calendarView);

			return containerView;
		}

		#endregion

		#endregion

		#region Private Methods

		private async void ReadContents(CustomCalendar calendarView) 
		{
			mDateItems = await SQLiteManager.Instance.LoadActivityDateListAsync (mActivityId);

			if (mDateItems == null || mDateItems.Count == 0) {
				ToastManager.ShowLongTime (Activity, Resource.String.toast_no_activity);
				return;
			}

			calendarView.ShowFromDate = mDateItems.OrderByDescending (item => item.Date).First ().Date;
			calendarView.CustomDataAdapter = mDateItems.Select (i => new CustomCalendarData (i.Date)).ToList ();
			calendarView.OnCalendarSelectedDate += (sender, e) => {
				var selectedItems = mDateItems.Where(item => {
					return e.SelectedDate.Year == item.Date.Year &&
						e.SelectedDate.Month == item.Date.Month &&
						e.SelectedDate.Day == item.Date.Day;
				}).ToList();

				if (selectedItems != null && selectedItems.Count > 0)
				{
					ShowToast(selectedItems);
				}
			};
		}

		private void ShowToast(IList<ActivityDateItem> items)
		{
			var item = items.Where (i => !String.IsNullOrEmpty(i.Memo)).FirstOrDefault();
			if (item != null) {
				ToastManager.ShowShortTime (Activity, item.Memo);
			} else {
				ToastManager.ShowShortTime (Activity, GetString(Resource.String.toast_no_memo));
			}
		}
			
		#endregion
	}
}

