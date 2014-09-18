using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

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
	}

}

