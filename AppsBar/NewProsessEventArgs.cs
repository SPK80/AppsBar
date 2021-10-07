using System.Diagnostics;
using System;

namespace AppsBar.Processes
{
	public class ProsessEventArgs : EventArgs
	{
		public Process Process { get; }
		public ProsessEventArgs(Process process)
		{
			this.Process = process;
		}

	}

	public delegate void ProsessEventHandler(object sender, ProsessEventArgs e);

}