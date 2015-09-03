using System;
using System.Collections.Generic;

namespace Scalarm
{
	public class TestGetSimErrorZeus : AbstractTestGetSimError
	{
		public TestGetSimErrorZeus()
		{
		}

		public override void scheduleWorkers(Experiment experiment)
		{
			experiment.ScheduleZeusJobs(1, new Dictionary<string, object> {
				{"queue", "plgrid-testing"},
				{"time_limit", 30}
			});
		}
	}
}

