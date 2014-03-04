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

namespace Com.Droibit.DidDo.Fragments
{
	/// <summary>
	/// Sort activity dialog fragment.
	/// </summary>
	public class SortActivityDialogFragment : DialogFragment
	{
		#region Inner Interface

		public interface Callbacks
		{
			void OnChoicedSortItem(int clickedPosition);
		}

		#endregion

		#region Private Fields

		#region Static

		private static readonly string TagDialog = "SortActivityDialogFragment";

		private static readonly string ArgItemPosition = "ItemPosition";

		#endregion

		private Callbacks mCallbacks;

		#endregion

		#region Public Methods

		#region Static

		public static SortActivityDialogFragment NewInstance(int itemPosition)
		{
			var args = new Bundle (1);
			args.PutInt (ArgItemPosition, itemPosition);

			var f = new SortActivityDialogFragment ();
			f.Arguments = args;
			return f;
		}

		#endregion

		#region DialogFragment

		/// <Docs>To be added.</Docs>
		/// <summary>
		/// Raises the attach event.
		/// </summary>
		/// <param name="activity">Activity.</param>
		public override void OnAttach (Activity activity)
		{
			base.OnAttach (activity);

			mCallbacks = TargetFragment as Callbacks;
			if (mCallbacks == null) {
				throw new InvalidCastException ("Parent fragment must implement fragment's callbacks.");
			}
		}

		/// <Docs>The last saved instance state of the Fragment,
		///  or null if this is a freshly created Fragment.</Docs>
		/// <returns>To be added.</returns>
		/// <summary>
		/// Raises the create dialog event.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
		public override Dialog OnCreateDialog (Bundle savedInstanceState)
		{
			int itemPosition = Arguments.GetInt (ArgItemPosition);

			return new AlertDialog.Builder (Activity)
					.SetSingleChoiceItems (Resource.Array.sort_activity_labels, itemPosition, (s, e) => {
						mCallbacks.OnChoicedSortItem(e.Which);
						Dismiss();
					}).Create ();
		}

		#endregion

		public void Show(Fragment targetFragment)
		{
			SetTargetFragment (targetFragment, 0);
			Show (targetFragment.FragmentManager, TagDialog);
		}

		#endregion
	}
}

