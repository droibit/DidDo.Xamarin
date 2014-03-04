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

namespace Com.Droibit.DidDo.Utils
{
	/// <summary>
	/// Toast manager.
	/// </summary>
	public static class ToastManager
	{
		#region Private Fields

		private static Toast OnlyToast;

		#endregion

		#region Public Methods

		public static void ShowShortTime (Context context, string text)
		{
			Show (context, text, ToastLength.Short);
		}

		public static void ShowShortTime(Context context, int resId)
		{
			Show (context, resId, ToastLength.Short);
		}

		public static void ShowLongTime(Context context, string text)
		{
			Show (context, text, ToastLength.Long);
		}

		public static void ShowLongTime(Context context, int resId)
		{
			Show (context, resId, ToastLength.Long);
		}

		#endregion

		#region Private Methods

		private static void Show(Context context, int resId, ToastLength length)
		{
			Show (context, context.GetString (resId), length);
		}

		private static void Show(Context context, string text, ToastLength length)
		{
			if (OnlyToast != null) {
				OnlyToast.Cancel ();
			}
			OnlyToast = Toast.MakeText (context, text, length);
			OnlyToast.Show ();
		}

		#endregion
	}
}

