using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;


namespace AppsBar
{
	public partial class MainForm : Form
	{
		private System.Timers.Timer timer;
		private System.Timers.Timer InitTimer(ElapsedEventHandler OnTimedEvent, double period = 500.0)
		{
			timer = new System.Timers.Timer(period);
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;
			return timer;
		}

		private Processes.AppsWatcher InitAppsWatcher(AppsView view)
		{
			var processes = new Processes.AppsWatcher();

			processes.NewProsess += (s, e) =>
			{
				var p = e.Process;
				view.Add(p);
			};

			processes.KilledProsess += (s, e) =>
			{
				var p = e.Process;
				view.Remove(p);
			};

			return processes;
		}

		public MainForm()
		{
			InitializeComponent();
			var updateButton = new Button();
			updateButton.Dock = DockStyle.Top;
			updateButton.Text = "Update";


			var appsView = new AppsView();
			appsView.Dock = DockStyle.Fill;
			Controls.Add(appsView);
			Controls.Add(updateButton);

			var appsWatcher = InitAppsWatcher(appsView);

			InitTimer((s, e) =>
			{
				Action action = () =>
				{
					if (!this.IsDisposed) appsWatcher.Update();
				};

				try
				{
					if (InvokeRequired) Invoke(action);
				}
				catch { }
			});

			updateButton.Click += (s, e) =>
			{
				appsWatcher.Update();
			};

		}

	}
}