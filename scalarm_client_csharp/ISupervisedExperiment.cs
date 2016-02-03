using System;
using System.Collections.Generic;

namespace Scalarm
{
	public interface ISupervisedExperiment : IExperiment
	{
		void SchedulePoint(ValuesMap point);

		void SchedulePoints(IEnumerable<ValuesMap> points);

		// TODO: values parameter (point) support
		void MarkAsComplete(string results, bool success = true, string errorReason = null);
	}
}

