using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;
using RestSharp;
using RestSharp.Deserializers;
using System.Net;

namespace Scalarm
{	
	public class SimulationScenario
    {
        public Client Client {get; set;}

        [DeserializeAs(Name = "_id")]
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

//        public SimulationScenario(string scenarioId, Client client)
//        {
//            InputSpecification = new List<Category>();
//            ScenarioId = scenarioId;
//            Client = client;
//        }

        // TODO: should create itself with Execute<SimulationScenario>
        public string GetDescription()
        {
            Client.ExecuteAsGet<SimulationScenario>(new RestRequest(), "/simulations/" + ScenarioId);
            throw new NotImplementedException();
        }

        public Experiment CreateExperiment(string experimentInput, Dictionary<string, object> experimentParams)
        {
            throw new NotImplementedException();
        }

        public Experiment CreateExperimentWithCSV(string experimentInput, Dictionary<string, object> experimentParams)
        {
            throw new NotImplementedException();
        }

        class ExperimentCreationResult
        {
            public string status {get; set;}
            public string experiment_id {get; set;}
            public string message {get; set;}
        }

        public Experiment CreateExperimentWithSinglePoint(Dictionary<string, float> point, Dictionary<string, object> parameters)
        {
            string csv = String.Join(",", point.Keys) + "\n" + string.Join(",", point.Values);

            var request = new RestRequest("experiments/start_import_based_experiment", Method.POST);
            // Add user additional parameters
            foreach (var p in parameters) request.AddParameter(p.Key, p.Value);
            // Create special usage-parameters for used experiment parameters
            foreach (var expParamName in point.Keys) request.AddParameter("param_" + expParamName, 1);
            // Add CSV file
            request.AddParameter("parameter_space_file", csv);
            request.AddParameter("simulation_id", ScenarioId);

            var response = Client.Execute<ExperimentCreationResult>(request);

            if (response.ResponseStatus != ResponseStatus.Completed) {
                throw new InvalidResponseStatusException(response);
            } else if (response.StatusCode != HttpStatusCode.OK) {
                throw new InvalidHttpStatusCodeException(response);
            }

            var responseData = response.Data;

            if (responseData.status == "ok")
            {
                return new Experiment(responseData.experiment_id, Client);
            } else if (responseData.status == "error") {
                throw new CreateExperimentException(responseData.message);
            } else {
                throw new InvalidResponseException(response);
            }
        }
	}

}

