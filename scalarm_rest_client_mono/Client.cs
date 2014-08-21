using System;
using System.Net;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using RestSharp;

namespace Scalarm
{	
	public class Client
	{
		public RestClient RestClient {get; private set;}
		
		public Client(string baseUrl, string login, string password)
		{
			ServicePointManager.ServerCertificateValidationCallback +=
        		(sender, certificate, chain, sslPolicyErrors) => true;
			
			RestClient = new RestClient();
			RestClient.BaseUrl = baseUrl;
			RestClient.Authenticator = new HttpBasicAuthenticator(login, password);
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

            var response = RestClient.Execute<ScalarmResource<SimulationScenario>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed) {
                throw new InvalidResponseStatusException(response);
            } else if (response.StatusCode != HttpStatusCode.OK) {
                throw new InvalidHttpStatusCodeException(response);
            }

            var resource = response.Data;

            if (resource.Status == "ok") {
                return resource.Data;
            } else if (resource.Status == "error") {
                throw new ScalarmResourceException(resource);
            } else {
                throw new InvalidResponseException(response);
            }
		}
	}

}

