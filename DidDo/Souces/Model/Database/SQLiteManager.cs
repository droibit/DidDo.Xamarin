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
using System.IO;
using Android.Content.Res;

using Environment = System.Environment;
using SQLite;
using System.Threading.Tasks;

namespace Com.Droibit.DidDo.Models
{
	/// <summary>
	/// SQ lite manager.
	/// </summary>
	public class SQLiteManager
	{
		#region Private Fields

		#region Static

		private static readonly string DatabaseName = "diddo.sqlite";

		#endregion

		private SQLiteAsyncConnection mConnection;

		#endregion

		#region Public Properties

		public static SQLiteManager Instance {
			get;
			private set;
		}

		#endregion

		#region Constructor

		static SQLiteManager()
		{
			Instance = new SQLiteManager ();
		}

		private SQLiteManager() {
		}

		#endregion

		#region Public Methods

		public void Open(AssetManager assets)
		{
			var sqlitePath = Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), DatabaseName);
			if (!File.Exists (sqlitePath)) {
				using (var reader = new BinaryReader(assets.Open(DatabaseName)))
				using (var writer = new BinaryWriter(new FileStream(sqlitePath, FileMode.CreateNew))) {
					var buffer = new byte[2056];
					int count = 0;
					while ((count = reader.Read(buffer, 0, buffer.Length)) > 0) {
						writer.Write (buffer, 0, count);
					}
				}
			}
			mConnection = new SQLiteAsyncConnection (sqlitePath);
		}

		public async Task<List<ActivityItem>> LoadActivityListAsync() 
		{
			return await mConnection.Table<ActivityItem> ().ToListAsync ();
		}

		public async Task<List<ActivityDateItem>> LoadActivityDateListAsync(int activityId)
		{
			return await mConnection.Table<ActivityDateItem> ()
					.Where (item => item.ActivityId == activityId)
					.OrderByDescending(item => item.Date)
					.ToListAsync ();
		}

		public async Task<ActivityItem> AddActivityAsync(ActivityItem item)
		{
			var resCount = await mConnection.InsertAsync (item);
			if (resCount == 0) {
				return null;
			}
			return await mConnection.Table<ActivityItem> ()
					.Where (i => i.Name == item.Name)
					.FirstAsync ();
		}

		public async Task<int> UpdateActivityAsync(ActivityItem item)
		{
			return await mConnection.UpdateAsync (item);
		}
			
		public async Task<int> DeleteActivityAsync(ActivityItem item)
		{
			int resCount = await mConnection.DeleteAsync (item);
			if (resCount == 0) {
				return 0;
			}

			var detailItems = await mConnection.Table<ActivityDateItem> ()
				.Where (d => d.ActivityId == item.Id)
				.ToListAsync ();
			foreach (var d in detailItems) {
				resCount += await mConnection.DeleteAsync (d);
			}
			return resCount;
		}

		public async Task<ActivityDateItem> AddActivityDateAsync(ActivityDateItem item)
		{
			var resCount = await mConnection.InsertAsync (item);
			if (resCount == 0) {
				return null;
			}

			resCount = await UpdateParentRecentlyDateAsync (item);
			if (resCount == 0) {
				return null;
			}

			return await mConnection.Table<ActivityDateItem> ()
					.Where (i => i.Date == item.Date && i.ActivityId == item.ActivityId)
					.FirstAsync ();
		}

		public async Task<int> DeleteActivityDateAsync(ActivityDateItem item)
		{
			int resCount = await mConnection.DeleteAsync (item);

			if (await UpdateParentRecentlyDateAsync (item) != 1) {
				return 0;
			}

			return resCount;
		}

		public async Task<int> UpdateActivityDateAsync(ActivityDateItem item)
		{
			return await mConnection.UpdateAsync (item);
		}

		public async Task<DateTime> GetRecentlyActivityDateAsync(int activityId)
		{
			var item = await mConnection.Table<ActivityDateItem> ()
				.OrderByDescending (i => i.Date)
				.FirstAsync ();
			if (item != null) {
				return item.Date;
			}
			return DateTime.Now;
		}

		private async Task<int> UpdateParentRecentlyDateAsync(ActivityDateItem item)
		{
			var activity = await mConnection.Table<ActivityItem> ()
				.Where (i => i.Id == item.ActivityId)
				.FirstAsync ();
			if (activity == null) {
				throw new InvalidOperationException ();
			}
			activity.RecentlyDate = item.Date;
			return await mConnection.UpdateAsync (activity);
		}

		#endregion
	}
}

