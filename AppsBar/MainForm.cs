using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace AppsBar
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			InitBar();
			InitTimer();

		}

		private void InitTimer()
		{
			timer = new System.Timers.Timer(1000.0);
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;
		}

		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			Action action = () => { if (!this.IsDisposed) { FillBar(); } };

			try
			{
				if (InvokeRequired) Invoke(action);
			}
			catch { }
		}

		private TreeView bar;
		private System.Timers.Timer timer;

		private void InitBar()
		{
			bar = new TreeView();
			bar.Dock = DockStyle.Fill;
			Controls.Add(bar);
		}

		private void OnBeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Level > 0)
			{
				bar.BeginUpdate();
				e.Cancel = true;
				var p = e.Node.Parent;
				bar.SelectedNode = p;
				bar.EndUpdate();
			}
		}
		private void FillBar()
		{
			var processes = Process.GetProcesses();
			bar.BeginUpdate();
			int selected = -1;
			if (bar.SelectedNode != null && bar.SelectedNode.Tag != null)
				selected = ((Process)bar.SelectedNode.Tag).Id;
			var expandeds = bar.Nodes.
			Cast<TreeNode>().
			Where(t => t.IsExpanded).
			Select(t => ((Process)t.Tag).Id).
			ToArray();

			bar.Nodes.Clear();
			bar.BeforeSelect += OnBeforeSelect;
			foreach (Process process in processes.Where(p => !p.MainWindowHandle.Equals(IntPtr.Zero)))
			{
				var n = bar.Nodes.Add(process.ProcessName);
				n.Tag = process;
				n.Nodes.Add(process.Id.ToString());
				n.Nodes.Add(process.MainWindowTitle == "" ? "(Пусто)" : process.MainWindowTitle);
				if (expandeds.Contains(process.Id))
					n.Expand();
				if (selected >= 0 && process.Id == selected)
					bar.SelectedNode = n;
			}
			bar.EndUpdate();
		}
	}
}
