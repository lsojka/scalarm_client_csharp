using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;
using RestSharp.Deserializers;
using System.Threading;

namespace Scalarm
{
	public class Application
	{	
		private static bool shouldWait = true;

		static void ShowResults(object sender, EventArgs e)
		{
			Console.WriteLine("Experiment done!");

			// retunrs dictionary
			var result = experiment.GetSingleResult(point);
			var productResult = result["product"];

			Console.WriteLine("Result for single point: " + productResult);

			shouldWait = false;
		}

		private static Experiment experiment;
		private static ValuesMap point;

		static void Main()
		{
			// TODO: it will be good to use camelCase param names that is not the same as REST params,
			// and convert these names to REST keys, e.g. input_writer -> inputWriterPath
            string configText = System.IO.File.ReadAllText("config.json");
            var config = JsonConvert.DeserializeAnonymousType(configText, new {base_url = "", login = "", password = ""});
			
			var client = new Client(config.base_url, config.login, config.password);
			
            var randomNum = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).Next(1000);

            var simulationName = String.Format("New simulation {0}", randomNum);
            var baseScenarioPath = "/home/kliput/Programowanie/scalarm/inne/moja-symulacja-double/";

            Func<string, string> scenarioPath = p => string.Format("{0}/{1}", baseScenarioPath, p);
            var simulationBinariesPath = scenarioPath("bin.zip");
            var executorPath = scenarioPath("executor.py");
            
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
                    new Parameter("parameter1", "Param 1") {
                        ParametrizationType = ExperimentInput.ParametrizationType.RANGE,
                        Type = ExperimentInput.Type.FLOAT,
                        Min = 0, Max = 1000
                    },
					new Parameter("parameter2", "Param 2") {
						ParametrizationType = ExperimentInput.ParametrizationType.RANGE,
						Type = ExperimentInput.Type.FLOAT,
						Min = -100, Max = 100
					}
                };

				SimulationScenario scenario = client.RegisterSimulationScenario(
					simulationName, simulationBinariesPath, simulationParameters, executorPath, scenarioParams);
				
                // Get existing scenario object
				// SimulationScenario scenario = client.GetScenarioById("54293cce20a6f123c5000038");
				
                Console.WriteLine("Got scenario with name: {0}, created at: {1}", scenario.Name, scenario.CreatedAt);

				// TODO: internally, get scenario input and mix with experiment-specific
				//Experiment experiment = scenario.CreateExperiment(experimentInput, experimentParams);

                point = new ValuesMap() {
                    {"parameter1", 3.0},
					{"parameter2", 4.0}
                };

                experiment = scenario.CreateExperimentWithSinglePoint(point, experimentParams);
				
                Console.WriteLine("Created experiment with ID: {0}", experiment.ExperimentId);

                var jobs = experiment.ScheduleZeusJobs(1);

                foreach (var j in jobs) {
                    Console.WriteLine("Scheduled: {0} {1}", j.Id, j.State);
                }

                // experiment.WaitForDone();

				experiment.ExperimentCompleted += ShowResults;
				experiment.WatchingIntervalSecs = 3;
				experiment.StartWatching();

				while (shouldWait) {
					Console.WriteLine("DEBUG: waiting...");
					Thread.Sleep(1000);
				}


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

