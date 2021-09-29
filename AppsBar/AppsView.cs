using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System;
using System.ComponentModel;

namespace AppsBar
{

	public class AppsView : ListView
	{

		private delegate void PropertyChangedAction(AppData sender, string propertyName);
		private class AppData
		{
			public AppData(Process process)
			{
				this.Id = process.Id;
				this.mainWindowTitle = process.MainWindowTitle;
			}


			public event PropertyChangedEventHandler Changed;

			public int Id { get; }
			private string mainWindowTitle;
			public string MainWindowTitle
			{
				get => mainWindowTitle;
				set
				{
					mainWindowTitle = value;
					Changed?.Invoke(this, new PropertyChangedEventArgs("MainWindowTitle"));
				}
			}

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
					var sameAppData = this.appsData.First(ad => ad.Id == process.Id);
					if (sameAppData.MainWindowTitle != process.MainWindowTitle)
						sameAppData.MainWindowTitle = process.MainWindowTitle;
				}
				catch (InvalidOperationException)
				{
					var newAppData = new AppData(process);

					var newItem = new ListViewItem(newAppData.ToString());
					newItem.Tag = newAppData;
					newAppData.Changed += (sender, e) =>
						{
							//if MainWindowTitle changed, update ListViewItem Text (contains changed AppData)

							if (sender == null) throw new ArgumentNullException();
							if (!(sender is AppData)) throw new ArgumentException("sender mast be AppData");
							var appData = sender as AppData;
							if (e.PropertyName == "MainWindowTitle") newItem.Text = appData.MainWindowTitle;

						};

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