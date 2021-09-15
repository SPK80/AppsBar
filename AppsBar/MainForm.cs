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
			// FillApps();
			timer = new System.Timers.Timer(500.0);
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;
		}

		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			Action action = () =>
			{
				try
				{
					if (!this.IsDisposed) FillApps();
				}
				catch { }
			};

			if (InvokeRequired)
				Invoke(action);

		}

		private TreeView bar;
		private System.Timers.Timer timer;

		private void InitBar()
		{
			bar = new TreeView();
			bar.Dock = DockStyle.Fill;
			// bar.Location = new Point(x, y);
			// bar.Width = this.Width - 1;
			// bar.Height = this.Height - b1.Height - 1;
			Controls.Add(bar);
		}

		private void FillApps()
		{
			var processes = Process.GetProcesses();
			bar.BeginUpdate();
			bar.Nodes.Clear();
			var pr = processes.Where(p => !p.MainWindowHandle.Equals(IntPtr.Zero));
			foreach (Process process in pr)
			{
				var n = bar.Nodes.Add(process.ProcessName);
				n.Nodes.Add(process.Id.ToString());
				n.Nodes.Add(process.MainWindowTitle == "" ? "(Пусто)" : process.MainWindowTitle);

			}
			bar.EndUpdate();
		}
	}
}
