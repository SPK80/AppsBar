using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System;

namespace AppsBar
{

	public class AppsView : ListView
	{
		private class AppData
		{

			public AppData(Process process)
			{
				this.Id = process.Id;
				this.MainWindowTitle = process.MainWindowTitle;
			}

			public int Id { get; }
			public string MainWindowTitle { get; }

			public override string ToString()
			{
				return this.MainWindowTitle == "" ? "[Пусто]" : this.MainWindowTitle;
			}

		}
		public AppsView()
		{
			this.View = View.List;
		}

		private IEnumerable<AppData> appsData
		{
			get => this.Items.
			Cast<ListViewItem>().
			Select(it => (it.Tag as AppData));
		}

		private AppData itemAppData(ListViewItem item)
		{
			return item.Tag as AppData;
		}

		public void Update(Process[] processes)
		{
			//add App if found new process
			foreach (var process in processes)
			{
				try
				{
					//var sameAppData = 
					this.appsData.First(ad => ad.Id == process.Id);
				}
				catch (InvalidOperationException)
				{
					var newAppData = new AppData(process);
					var newItem = new ListViewItem(newAppData.ToString());
					newItem.Tag = newAppData;
					this.Items.Add(newItem);
				}
			}

			//find and remove killed Apps
			foreach (ListViewItem item in this.Items)
			{
				try
				{
					processes.First(p => p.Id == itemAppData(item).Id);
				}
				catch (InvalidOperationException)
				{
					this.Items.Remove(item);
				}
			}

		}

	}

}