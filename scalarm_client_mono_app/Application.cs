using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;
using RestSharp.Deserializers;

namespace Scalarm
{
	public class Application
	{		
		static void Main()
		{
			// TODO: it will be good to use camelCase param names that is not the same as REST params,
			// and convert these names to REST keys, e.g. input_writer -> inputWriterPath
            string configText = System.IO.File.ReadAllText("config.json");
            var config = JsonConvert.DeserializeAnonymousType(configText, new {base_url = "", login = "", password = ""});
			
			var client = new Client(config.base_url, config.login, config.password);
			
			var experimentParams = new Dictionary<string, object>()
			{
				{"experiment_name", "Some experiment"},
				{"experiment_description", "This is a dummy experiment"},
				// {"doe", experimentDoe},
				{"execution_time_constraint", 3600}
			};
			
			try {
				// TODO: below method with executor id instead of path overload
//				SimulationScenario scenario = client.registerSimulationScenario(
//					simulationName, simulationBinariesPath, simulationInputPath, executorPath, simulationRegisterParams);
				
				// TODO: get scenario with ID
				SimulationScenario scenario = client.GetScenarioById("53ecace520a6f1565500000c");
				
                Console.WriteLine("Got scenario with name: {0}, created at: {1}", scenario.Name, scenario.CreatedAt);

				// TODO: internally, get scenario input and mix with experiment-specific
				//Experiment experiment = scenario.CreateExperiment(experimentInput, experimentParams);

                var point = new Dictionary<string, float>() {
                    {"main_category___main_group___parameter1", 3},
                    {"main_category___main_group___parameter2", 4},
                };

                Experiment experiment = scenario.CreateExperimentWithSinglePoint(point, experimentParams);
				
                Console.WriteLine("Created experiment with ID: {0}", experiment.ExperimentId);

			} catch (RegisterSimulationScenarioException e) {
				Console.WriteLine("Registering simulation scenario failed: " + e);
			} catch (CreateExperimentException e) {
				Console.WriteLine("Creating experiment failed: " + e);
			} catch (InvalidResponseException e) {
                Console.WriteLine("Invalid response: {0};\n\n{1};\n\n{2}", e.Response.Content, e.Response.ErrorMessage, e.Response.ErrorException);
            } catch (ScalarmResourceException<SimulationScenario> e) {
                Console.WriteLine("Error getting Scalarm SimulationScenario resource: {0}", e.Resource.ErrorCode);
            } catch (FileNotFoundException e) {
                Console.WriteLine("Configuration file not found ({0})", e.FileName);
            }
		}
	}
}

