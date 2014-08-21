using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;
using RestSharp;
using RestSharp.Deserializers;

namespace Scalarm
{	
	public class SimulationScenario
    {
        public Client Client {get; private set;}

        [JsonProperty(PropertyName = "_id")]
        public string ScenarioId {get; private set;}

        [JsonProperty(PropertyName = "name")]
        public string Name {get; private set;}

        [JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description {get; private set;}

        [JsonProperty(PropertyName = "input_specification")]
        public List<Category> InputSpecification {get; private set;}

        [JsonProperty(PropertyName = "user_id")]
        public string UserId {get; private set;}

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt {get; private set;}

        [JsonProperty(PropertyName = "input_writer_id")]
        public string InputWriterId {get; private set;}

        [JsonProperty(PropertyName = "executor_id")]
        public string ExecutorId {get; private set;}

        [JsonProperty(PropertyName = "output_reader_id")]
        public string OutputReaderId {get; private set;}

        [JsonProperty(PropertyName = "progress_monitor_id")]
        public string ProgressMonitorId {get; private set;}

        [JsonProperty(PropertyName = "simulation_binaries_id")]
        public string SimulationBinariesId {get; private set;}

        public SimulationScenario()
        {}

        public SimulationScenario(string scenarioId, Client client)
        {
            InputSpecification = new List<Category>();
            ScenarioId = scenarioId;
            Client = client;
        }

        // TODO: should create itself with Execute<SimulationScenario>
        public string GetDescription()
        {
            Client.RestClient.ExecuteAsGet<SimulationScenario>(new RestRequest(), "/simulations/" + ScenarioId);
            throw new NotImplementedException();
        }

        public Experiment CreateExperiment(string experimentInput, Dictionary<string, object> experimentParams)
        {
            throw new NotImplementedException();
        }
	}

}

