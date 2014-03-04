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
using Com.Droibit.DidDo.Utils;

namespace Com.Droibit.DidDo.Fragments
{	
	/// <summary>
	/// Add activity dialog fragment.
	/// </summary>
	public class AddActivityDialogFragment : DialogFragment, View.IOnKeyListener
	{
		#region Inner Interface

		public interface Callbacks
		{
			bool IsNewActivityName (string activityName);
			void OnEnteredNewActivity(string activityName);
		}

		#endregion

		#region Private Fields

		#region Static

		private static readonly string TagDialog = "AddActivityDialogFragment";

		#endregion

		private Callbacks mCallbacks;

		#endregion

		#region Public Methods

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
			var contentView = LayoutInflater.From (Activity).Inflate (
				Resource.Layout.DialogAddActivity, null);
			var editText = contentView.FindViewById<EditText> (Resource.Id.edit_activity_text);
			editText.SelectAll ();
			editText.SetOnKeyListener (this);

			var builder = new AlertDialog.Builder (Activity)
				.SetTitle (Resource.String.dialog_title_add_activity)
				.SetView (contentView)
				.SetPositiveButton (Android.Resource.String.Ok, (s, e) => {
					if (InvalidateActivityName(editText.Text)) {
						mCallbacks.OnEnteredNewActivity(editText.Text);
					}
				})
				.SetNegativeButton (Android.Resource.String.Cancel, (s, e) => {
					Dismiss();
				});
			var dialog = builder.Create();
			dialog.Window.SetSoftInputMode (SoftInput.StateVisible);

			return dialog;
		}

		#endregion

		#region IOnKeyListener

		public bool OnKey (View v, Keycode keyCode, KeyEvent e)
		{
			if (e.Action == KeyEventActions.Down && keyCode == Keycode.Enter) {
				var editText = v as EditText;
				if (InvalidateActivityName(editText.Text)) {
					mCallbacks.OnEnteredNewActivity(editText.Text);
					Dismiss ();
				}
				return true;
			}
			return false;
		}

		#endregion

		public void Show(Fragment targetFragment)
		{
			SetTargetFragment (targetFragment, 0);
			Show (targetFragment.FragmentManager, TagDialog);
		}

		#endregion

		#region Private Methods

		private bool InvalidateActivityName(string activityName)
		{
			if (String.IsNullOrEmpty (activityName)) {
				ToastManager.ShowShortTime (Activity, Resource.String.toast_input_result_empty_activity_name);
				return false;
			} else if (!mCallbacks.IsNewActivityName (activityName)) {
				ToastManager.ShowShortTime (Activity, Resource.String.toast_input_result_duplicate_activity_name);
				return false;
			}
			return true;
		}

		#endregion
	}
}

