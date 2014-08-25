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
		public SimulationScenario registerSimulationScenario(
			string simulationName, string simulationBinariesPath, string simulationInputPath, 
			string executorPath, Dictionary<string, object> simulationRegisterParams)
		{
			throw new NotImplementedException();
		}

		public SimulationScenario GetScenarioById(string scenarioId)
		{
            var request = new RestRequest("/simulation_scenarios/{id}", Method.GET);
            request.AddUrlSegment("id", scenarioId);

            var response = this.Execute<ScalarmResource<SimulationScenario>>(request);

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
                throw new ScalarmResourceException(resource);
            } else {
                throw new InvalidResponseException(response);
            }
		}
	}

}

