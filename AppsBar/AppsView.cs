using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

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

		public void Update(Process[] processes)
		{

			var selId = this.SelectedItems.
			Cast<ListViewItem>().
			Select(it => (it.Tag as AppData).Id).
			ToArray();

			this.BeginUpdate();
			this.Items.Clear();
			foreach (Process process in processes)
			{
				var it = new ListViewItem();
				AppData appData = (AppData)(it.Tag = new AppData(process));
				it.Text = appData.ToString();
				if (selId.Contains(appData.Id))
					it.Selected = true;
				this.Items.Add(it);

			}
			this.EndUpdate();

		}

	}

}