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
using Java.IO;

namespace Com.Droibit.DidDo.Fragments
{
	/// <summary>
	/// Modify text dialog fragment.
	/// </summary>
	public class ModifyTextDialogFragment : DialogFragment, View.IOnKeyListener
	{
		#region Inner Classes

		public class Info : Java.Lang.Object, ISerializable
		{
			public string Text {
				get;
				set;
			}

			public int TargetPosition {
				get;
				set;
			}

			public int DialogTitleId {
				get;
				set;
			}

			public int ViewId {
				get;
				set;
			}
		}

		public interface Callbacks
		{
			void OnModifiedText(int targetPosition, string modifiedText);
		}

		#endregion

		#region Private Fields

		#region Static

		private static readonly string ArgSerializableInfo = "ArgSerializableInfo";

		private static readonly string TagDialog = "ModifyTextDialogFragment";

		#endregion

		private Callbacks mCallbacks;

		#endregion

		#region Constructor

		public ModifyTextDialogFragment() 
		{
		}

		#endregion

		#region Public Methods

		#region Static

		public static ModifyTextDialogFragment NewInstance(Info info)
		{
			var args = new Bundle (1);
			args.PutSerializable (ArgSerializableInfo, info);

			var f = new ModifyTextDialogFragment ();
			f.Arguments = args;
			return f;
		}

		#endregion

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
			var info = Arguments.GetSerializable (ArgSerializableInfo) as Info;
			var contentView = LayoutInflater.From (Activity).Inflate (
				info.ViewId, null);
			var editText = contentView.FindViewById<EditText> (Resource.Id.edit_activity_text);
			editText.Text = info.Text;
			editText.SelectAll ();
			editText.SetOnKeyListener (this);

			var builder = new AlertDialog.Builder (Activity)
				.SetTitle (info.DialogTitleId)
				.SetView (contentView)
				.SetPositiveButton (Android.Resource.String.Ok, (s, e) => {
					mCallbacks.OnModifiedText (info.TargetPosition, editText.Text);
				})
				.SetNegativeButton (Android.Resource.String.Cancel, (s, e) => {
					Dismiss();
				});
			var dialog = builder.Create ();
			dialog.Window.SetSoftInputMode (SoftInput.StateVisible);

			return dialog;
		}

		#endregion

		#region IOnKeyListener

		public bool OnKey (View v, Keycode keyCode, KeyEvent e)
		{
			if (e.Action == KeyEventActions.Down && keyCode == Keycode.Enter) {
				var editText = v as EditText;
				var info = Arguments.GetSerializable (ArgSerializableInfo) as Info;
				mCallbacks.OnModifiedText (info.TargetPosition, editText.Text);
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
	}
}

