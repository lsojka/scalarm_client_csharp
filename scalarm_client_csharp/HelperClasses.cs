using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Scalarm
{
    public class ExperimentCreationResult
    {
        public string status {get; set;}
        public string experiment_id {get; set;}
        public string message {get; set;}
    }

    public class ScenarioCreationResult
    {
        public string status {get; set;}
        public string simulation_id {get; set;}
        public string message {get; set;}
    }

    public class ScheduleSimulationManagersResult
    {
        public string status {get; set;}
        public string msg {get; set;}
        public string error_code {get; set;}
        public string infrastructure {get; set;}
        public List<string> records_ids {get; set;}
    }

    public class SimulationManagerResource
    {
        public string status {get; set;}
        public SimulationManager record {get; set;}
    }

	public class SimulationManagersList
	{
		public string status { get; set; }
		public List<SimulationManager> sm_records { get; set; }
	}

	public class AddCredentialsResult
	{
		public string status { get; set; }
		public string record_id { get; set; }
		public string msg { get; set; }
		public string error_code { get; set; }
	}

	public class SimulationManagerCommandResult
	{
		public string status { get; set; }
		public string msg { get; set; }
		public string cmd { get; set; }
		public string error_code { get; set; }
	}

	public class SchedulePointResult
	{
		public string status { get; set; }
	}

	// TODO: merge with other, change name
	public class ScalarmStatus
	{
		public string status { get; set; }
	}

	public class ExperimentsListResult
	{
		public string status { get; set; }
		public List<string> running { get; set; }
		public List<string> completed { get; set; }
		public List<string> historical { get; set; }
	}
}
   public class NurbsIntermediateResult
    {
       public List<List<string>> aaData { get; set; }

       public string sim_id { get; set; }
       public string timestamp { get; set; }
       public string min_error { get; set; }

       public void ParseAaDaata()
       {
           sim_id = aaData[0][0];
           timestamp = aaData[0][1];
           string pat = @"([0-9]+\.[0-9])+\w+";
           Regex r = new Regex(pat, RegexOptions.IgnoreCase);
           Match m = r.Match(aaData[0][2]);
           min_error = m.Groups[0].ToString();

       }
    }
