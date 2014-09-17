using System;
using System.Net;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using RestSharp;

namespace Scalarm
{	
	public class Client : RestClient
	{
		public Client(string baseUrl, string login, string password)
		{
			ServicePointManager.ServerCertificateValidationCallback +=
        		(sender, certificate, chain, sslPolicyErrors) => true;
			
            this.BaseUrl = baseUrl;
			this.Authenticator = new HttpBasicAuthenticator(login, password);
            // Cannot use this because of bug in JSON.net for Mono!
            // this.AddHandler("application/json", new JsonConvertDeserializer());
		}

		public SimulationScenario GetScenarioById(string scenarioId)
		{
            var request = new RestRequest("/simulation_scenarios/{id}", Method.GET);
            request.AddUrlSegment("id", scenarioId);

            var response = this.Execute<ResourceEnvelope<SimulationScenario>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed) {
                throw new InvalidResponseStatusException(response);
            } else if (response.StatusCode != HttpStatusCode.OK) {
                throw new InvalidHttpStatusCodeException(response);
            }

            var resource = response.Data;

            if (resource.Status == "ok") {
                SimulationScenario scenario = resource.Data;
                scenario.Client = this;
                return scenario;
            } else if (resource.Status == "error") {
                throw new ScalarmResourceException<SimulationScenario>(resource);
            } else {
                throw new InvalidResponseException(response);
            }
		}

        public Experiment GetExperimentById(string experimentId)
        {
            // TODO: fetch full info about experiment and deserialize

            Experiment experiment = new Experiment(experimentId, this);

            // TODO!
            return null;
        }

        // TODO: handle wrong scenario id
        public Experiment CreateExperimentWithSinglePoint(
            string simulationScenarioId,
            Dictionary<string, float> point,
            Dictionary<string, object> additionalParameters = null
            )
        {
            string csv = String.Join(",", point.Keys) + "\n" + string.Join(",", point.Values);

            var request = new RestRequest("experiments/start_import_based_experiment", Method.POST);
            // Add user additional parameters
            if (additionalParameters != null) {
                foreach (var p in additionalParameters) request.AddParameter(p.Key, p.Value);
            }
            // Create special usage-parameters for used experiment parameters
            foreach (var expParamName in point.Keys) request.AddParameter("param_" + expParamName, 1);
            // Add CSV file
            request.AddParameter("parameter_space_file", csv);
            request.AddParameter("simulation_id", simulationScenarioId);

            var response = this.Execute<ExperimentCreationResult>(request);
            return HandleExperimentCreationResponse(response);
        }

        private Experiment HandleExperimentCreationResponse(IRestResponse<ExperimentCreationResult> response)
        {
            ValidateResponseStatus(response);

            var creationResult = response.Data;

            if (creationResult.status == "ok")
            {
                return new Experiment(creationResult.experiment_id, this);
            } else if (creationResult.status == "error") {
                throw new CreateScenarioException(creationResult.message);
            } else {
                throw new InvalidResponseException(response);
            }
        }

        protected void ValidateResponseStatus(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed) {
                throw new InvalidResponseStatusException(response);
            } else if (response.StatusCode != HttpStatusCode.OK) {
                throw new InvalidHttpStatusCodeException(response);
            }
        }

        public SimulationScenario RegisterSimulationScenario(
              string simulationName,
              string simulationBinariesPath,
              string simulationInputPath,
              string executorPath,
              Dictionary<string, object> attributes = null) {

            var simulationInput = System.IO.File.ReadAllText(simulationInputPath);

            return _registerSimulationScenario(simulationName, simulationBinariesPath, simulationInput, executorPath, attributes);
        }

        public SimulationScenario RegisterSimulationScenario(
              string simulationName,
              string simulationBinariesPath,
              List<ExperimentInput.Parameter> simpleInputDefinition,
              string executorPath,
              Dictionary<string, object> attributes = null) {


            return _registerSimulationScenario(simulationName, simulationBinariesPath,
                                               _createInputDefinition(simpleInputDefinition),
                                               executorPath);
        }

        private string _createInputDefinition(List<ExperimentInput.Parameter> simpleInputDefinition) {
            var cats = new List<ExperimentInput.Category>() {
                new ExperimentInput.Category(null, null) { Entities = new List<ExperimentInput.Entity>() {
                        new ExperimentInput.Entity(null, null) { Parameters = new List<ExperimentInput.Parameter>() }
                    }}
            };

            foreach (var p in simpleInputDefinition) {
                cats[0].Entities[0].Parameters.Add(p);
            }

            return JsonConvert.SerializeObject(cats);
        }

        public SimulationScenario _registerSimulationScenario(
              string simulationName,
              string simulationBinariesPath,
              string simulationInputDefinition,
              string executorPath,
              Dictionary<string, object> attributes = null) {
              
            var request = new RestRequest("simulations", Method.POST);
                         
            request.AddParameter("simulation_name", simulationName);
              
            request.AddFile("simulation_binaries", simulationBinariesPath);
            request.AddParameter("simulation_input", simulationInputDefinition);
            request.AddFile("executor", executorPath);

            if (attributes != null) {
                foreach (var p in new string[] {"input_writer", "output_reader", "progress_monitor"}) {
                    if (attributes.ContainsKey(p)) {
                        request.AddFile(p, (string)attributes[p]);
                        attributes.Remove(p);
                    }
                }
    
                foreach (var entry in attributes) {
                    request.AddParameter(entry.Key, entry.Value);
                }
            }

            var response = this.Execute<ScenarioCreationResult>(request);
            return HandleScenarioCreationResponse(response);

        }

        private SimulationScenario HandleScenarioCreationResponse(IRestResponse<ScenarioCreationResult> response) {
            ValidateResponseStatus(response);

            var creationResult = response.Data;

            if (creationResult.status == "ok")
            {
                return GetScenarioById(creationResult.simulation_id);
            } else if (creationResult.status == "error") {
                throw new CreateScenarioException(creationResult.message);
            } else {
                throw new InvalidResponseException(response);
            }
        }
	}

}

