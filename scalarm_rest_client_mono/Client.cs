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

		/// <exception cref="RegisterSimulationScenarioException">On error</exception>
		public SimulationScenario RegisterSimulationScenario(
			string simulationName, string simulationBinariesPath, string simulationInputPath, 
			string executorPath, Dictionary<string, object> simulationRegisterParams)
		{
			throw new NotImplementedException();
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
            Dictionary<string, object> additionalParameters
            )
        {
            string csv = String.Join(",", point.Keys) + "\n" + string.Join(",", point.Values);

            var request = new RestRequest("experiments/start_import_based_experiment", Method.POST);
            // Add user additional parameters
            foreach (var p in additionalParameters) request.AddParameter(p.Key, p.Value);
            // Create special usage-parameters for used experiment parameters
            foreach (var expParamName in point.Keys) request.AddParameter("param_" + expParamName, 1);
            // Add CSV file
            request.AddParameter("parameter_space_file", csv);
            request.AddParameter("simulation_id", simulationScenarioId);

            var response = this.Execute<ExperimentCreationResult>(request);

            if (response.ResponseStatus != ResponseStatus.Completed) {
                throw new InvalidResponseStatusException(response);
            } else if (response.StatusCode != HttpStatusCode.OK) {
                throw new InvalidHttpStatusCodeException(response);
            }

            var creationResult = response.Data;

            if (creationResult.status == "ok")
            {
                return new Experiment(creationResult.experiment_id, this);
            } else if (creationResult.status == "error") {
                throw new CreateExperimentException(creationResult.message);
            } else {
                throw new InvalidResponseException(response);
            }
        }
	}

}

