using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AppsBar.Processes
{
	public class AppsWatcher
	{
		public event ProsessEventHandler NewProsess;
		public event ProsessEventHandler KilledProsess;

		private List<Process> processes = new List<Process>();

		public void Update()
		{
			var winProcesses = Process.GetProcesses().
			Where(p => !p.MainWindowHandle.Equals(IntPtr.Zero)).ToArray();

			var newProcesses = winProcesses.
			Where(p => processes.FirstOrDefault((f) => f.Id == p.Id) == null).
			ToList();

			processes.AddRange(newProcesses);
			newProcesses.ForEach(p => NewProsess?.Invoke(this, new ProsessEventArgs(p)));

			var killedProsesses = processes.Where(p => winProcesses.FirstOrDefault((f) => f.Id == p.Id) == null).
			ToList();

			killedProsesses.ForEach(p =>
			{
				processes.Remove(p);
				KilledProsess?.Invoke(this, new ProsessEventArgs(p));
			});

		}
	}
}