using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;
using System.Threading;

namespace Scalarm
{	
	public class Experiment : ScalarmObject
	{
        // TODO: make full experiment model

        public string ExperimentId {get; private set;}

        public Experiment(string experimentId, Client client) : base(client)
        {
            ExperimentId = experimentId;
        }

        public List<SimulationManager> ScheduleSimulationManagers(string infrastructure, int count, Dictionary<string, string> parameters) {
            return Client.ScheduleSimulationManagers(ExperimentId, infrastructure, count, parameters);
        }

        public List<SimulationManager> ScheduleZeusJobs(int count)
        {
            return ScheduleSimulationManagers("qsub", count, new Dictionary<string, string> {
                {"time_limit", "60"}
            });
        }

        public ExperimentStatistics GetStatistics()
        {
            return Client.GetExperimentStatistics(ExperimentId);
        }

        public bool IsDone()
        {
            var stats = GetStatistics();
			Console.WriteLine("DEBUG: exp stats: " + stats.ToString());
            return stats.All == stats.Done;
        }

        // TODO: check if there are workers running - if not - throw exception!
        /// <summary>
        ///  Actively waits for experiment for completion. 
        /// </summary>
        public void WaitForDone(int timeoutSecs=-1, int pollingIntervalSeconds=5)
        {
            var startTime = DateTime.UtcNow;

            while (timeoutSecs <= 0 || (DateTime.UtcNow - startTime).TotalSeconds < timeoutSecs) {
                if (IsDone()) {
                    return;
                }
                Thread.Sleep(pollingIntervalSeconds*1000);
            }
            throw new TimeoutException();
        }
	}

}

