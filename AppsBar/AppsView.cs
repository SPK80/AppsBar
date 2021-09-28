using System.Diagnostics;
using System.Windows.Forms;

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
			var sel = this.SelectedItems;
			this.BeginUpdate();
			this.Items.Clear();
			foreach (Process process in processes)
			{
				var it = new ListViewItem();
				it.Tag = new AppData(process);
				it.Text = (it.Tag as AppData).ToString();
				this.Items.Add(it);

			}
			this.EndUpdate();

		}

	}

}