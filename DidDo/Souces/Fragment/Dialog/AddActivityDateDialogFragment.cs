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

namespace Com.Droibit.DidDo.Fragments
{
	/// <summary>
	/// Add activity date dialog fragment.
	/// </summary>
	public class AddActivityDateDialogFragment : DialogFragment, View.IOnKeyListener
	{
		#region Inner Interface

		public interface Callbacks
		{
			void OnEnteredActivityDate(string memo);
		}

		#endregion

		#region Private Fields

		#region Static

		private static readonly string TagDialog = "AddActivityDateDialogFragment";

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
				Resource.Layout.DialogAddActivityDate, null);
			var editText = contentView.FindViewById<EditText> (Resource.Id.edit_activity_text);
			editText.SetOnKeyListener (this);

			var builder = new AlertDialog.Builder (Activity)
				.SetTitle (Resource.String.dialog_title_add_activity_date)
				.SetView (contentView)
				.SetPositiveButton (Android.Resource.String.Ok, (s, e) => {
					mCallbacks.OnEnteredActivityDate (GetMemo(editText.Text));
				})
				.SetNegativeButton (Android.Resource.String.Cancel, (s, e) => {
					Dismiss();
				});
			return builder.Create();
		}

		#endregion

		#region IOnKeyListener

		public bool OnKey (View v, Keycode keyCode, KeyEvent e)
		{
			if (e.Action == KeyEventActions.Down && keyCode == Keycode.Enter) {
				var editText = v as EditText;
				mCallbacks.OnEnteredActivityDate (GetMemo(editText.Text));
				Dismiss ();
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

		private string GetMemo(string rawMemo)
		{
			if (String.IsNullOrEmpty(rawMemo)) {
				return "---";
			}
			return rawMemo;
		}

		#endregion
	}
}

