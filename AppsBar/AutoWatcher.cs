using System.Timers;

namespace AppsBar.Processes
{
	public class AutoWatcher : AppsWatcher
	{
		public AutoWatcher(double updatePeriod = 500.0)
		{
			var timer = new Timer(updatePeriod);
			timer.Elapsed += (s, e) =>
			{
				Update();
			};

			timer.AutoReset = true;
			timer.Enabled = true;
		}
	}
}