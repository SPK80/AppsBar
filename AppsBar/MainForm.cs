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
			var updateButton = new Button();
			updateButton.Dock = DockStyle.Top;
			updateButton.Text = "Update";


			Controls.Add(updateButton);

			var appsView = new AppsView();
			appsView.Dock = DockStyle.Fill;
			Controls.Add(appsView);

			var processes = new Processes.AppsWatcher();
			processes.NewProsess += (s, e) =>
			{
				var p = e.Process;
				appsView.Add(p);
			};

			updateButton.Click += (s, e) =>
			{
				processes.Update();
			};

			// processes.KilledProsess += (s, e) =>
			// {
			// 	var p = e.Process;
			// 	appsView.Kill(p);
			// };

		}

	}
}