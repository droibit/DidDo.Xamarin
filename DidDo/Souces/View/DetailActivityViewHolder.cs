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

namespace Com.Droibit.DidDo.Views
{
	/// <summary>
	/// 活動の詳細ビュー
	/// </summary>
	public class DetailActivityViewHolder
	{
		#region Public Properties

		public View View {
			get;
			private set;
		}

		public TextView ElapsedDateView {
			get;
			private set;
		}

		public ListView ListView {
			get;
			private set;
		}

		public ActivityDateListAdapter ListAdapter {
			get;
			private set;
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Com.Droibit.DidDo.Views.DetailActivityViewHolder"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="containerView">Container view.</param>
		public DetailActivityViewHolder (Context context, ViewGroup containerView)
		{
			View = LayoutInflater.From (context).Inflate (
				Resource.Layout.FragmentDetail, containerView, false);

			ElapsedDateView = View.FindViewById<TextView> (Resource.Id.elapsed_date);
			ElapsedDateView.Text = context.GetString (Resource.String.format_elapsed_date, 0);

			ListView = View.FindViewById<ListView> (Android.Resource.Id.List);
			ListView.EmptyView = View.FindViewById (Android.Resource.Id.Empty);
			ListView.Adapter = ListAdapter = new ActivityDateListAdapter (context);
		}
			
		#endregion
	}
}

