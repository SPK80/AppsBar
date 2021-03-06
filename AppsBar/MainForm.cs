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
			timer = new System.Timers.Timer(500.0);
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;
		}

		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			Action action = () => { if (!this.IsDisposed) { UpdateBar(); } };

			try
			{
				if (InvokeRequired) Invoke(action);
			}
			catch { }
		}


		private AppsView bar;

		private System.Timers.Timer timer;

		private void InitBar()
		{
			bar = new AppsView();
			bar.Dock = DockStyle.Fill;
			Controls.Add(bar);
		}

		private void UpdateBar()
		{
			var processes = Process.GetProcesses().Where(p => !p.MainWindowHandle.Equals(IntPtr.Zero)).ToArray();
			bar.Update(processes);

		}
	}
}
