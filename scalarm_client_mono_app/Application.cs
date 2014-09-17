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
			
            var randomNum = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).Next(1000);

            var simulationName = String.Format("New simulation {0}", randomNum);
            var baseScenarioPath = "/home/kliput/Programowanie/scalarm/inne/moja-symulacja-0/";

            Func<string, string> scenarioPath = p => string.Format("{0}/{1}", baseScenarioPath, p);
            var simulationBinariesPath = scenarioPath("bin.zip");
            var executorPath = scenarioPath("executor.py");
            var inputDefinition = new Dictionary<string, object>() {};

            var scenarioParams = new Dictionary<string, object>() {};

			var experimentParams = new Dictionary<string, object>()
			{
                {"experiment_name", String.Format("New experiment {0}", randomNum)},
				{"experiment_description", "This is a dummy experiment"},
				// {"doe", experimentDoe},
				{"execution_time_constraint", 3600}
			};
			
			try {
				// TODO: below method with executor id instead of path overload

                var simulationParameters = new List<ExperimentInput.Parameter>() {
                    new Parameter("p1", "Param 1") {
                        ParametrizationType = ExperimentInput.ParametrizationType.RANGE,
                        Type = ExperimentInput.Type.FLOAT,
                        Min = 0, Max = 1000
                    }
                };

				SimulationScenario scenario = client.RegisterSimulationScenario(
					simulationName, simulationBinariesPath, simulationParameters, executorPath, scenarioParams);
				
				// SimulationScenario scenario = client.GetScenarioById("53ecace520a6f1565500000c");
				
                Console.WriteLine("Got scenario with name: {0}, created at: {1}", scenario.Name, scenario.CreatedAt);

				// TODO: internally, get scenario input and mix with experiment-specific
				//Experiment experiment = scenario.CreateExperiment(experimentInput, experimentParams);

                var point = new Dictionary<string, float>() {
                    {"p1", 3}
                };

                Experiment experiment = scenario.CreateExperimentWithSinglePoint(point, experimentParams);
				
                Console.WriteLine("Created experiment with ID: {0}", experiment.ExperimentId);

			} catch (RegisterSimulationScenarioException e) {
				Console.WriteLine("Registering simulation scenario failed: " + e);
			} catch (CreateScenarioException e) {
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

