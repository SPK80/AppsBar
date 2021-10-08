using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Drawing;

namespace AppsBar
{
	public class AppsView : ListView, IAppsView
	{
		public AppsView()
		{
			this.View = View.List;
		}

		public void Add(int pid)
		{
			var newItem = new ListViewItem();
			newItem.Tag = pid;
			Items.Add(newItem);
		}
		private ListViewItem findItem(int pid)
		{
			return Items.Cast<ListViewItem>().FirstOrDefault(it => (int)it.Tag == pid);
		}
		public void Remove(int pid)
		{
			var removingItem = findItem(pid);
			if (removingItem != null) Items.Remove(removingItem);
		}

		public void Update(int pid, string MainWindowTitle)
		{
			var item = findItem(pid);
			if (item != null)
				item.Text = MainWindowTitle;
		}
	}

	public interface IAppsView
	{
		void Add(int pid);
		void Remove(int pid);
		void Update(int pid, string MainWindowTitle);
	}
}