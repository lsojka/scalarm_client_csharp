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

		static void ShowResults(object sender, IList<SimulationParams> results)
		{
			Console.WriteLine("Experiment done!");

			// TODO: consider sending Results object (sender is Experiment)

			Console.WriteLine("Results:");
			foreach (var r in results) {
				Console.WriteLine(string.Format("{0} -> {1}", r.Input, r.Output));
			}

			shouldWait = false;
		}

		//private static Experiment experiment;
		private static List<ValuesMap> points = new List<ValuesMap>();

		public static string ReadPassword()
		{
			string pass = "";
			Console.Write("Enter your PL-Grid password: ");
			ConsoleKeyInfo key;

			do
			{
				key = Console.ReadKey(true);

				// Backspace Should Not Work
				if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
				{
					pass += key.KeyChar;
					Console.Write("*");
				}
				else
				{
					if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
					{
						pass = pass.Substring(0, (pass.Length - 1));
						Console.Write("\b \b");
					}
				}
			}
			// Stops Receving Keys Once Enter is Pressed
			while (key.Key != ConsoleKey.Enter);

			Console.WriteLine("\n");

			return pass;
		}

		static void Main()
		{	
			string plgPass = ReadPassword();

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
				// Add private machine credentials
				var creds = client.AddPrivateMachineCredentials("localhost", "user1", "pass");

				Console.WriteLine("added");

				throw new Exception("aa");

				// TODO: below method with executor id instead of path overload

				// define input parameters specification
//                var simulationParameters = new List<ExperimentInput.Parameter>() {
//                    new Parameter("parameter1", "Param 1") {
//                        ParametrizationType = ExperimentInput.ParametrizationType.RANGE,
//                        Type = ExperimentInput.Type.FLOAT,
//                        Min = 0, Max = 1000
//                    },
//					new Parameter("parameter2", "Param 2") {
//						ParametrizationType = ExperimentInput.ParametrizationType.RANGE,
//						Type = ExperimentInput.Type.FLOAT,
//						Min = -100, Max = 100
//					}
//                };

				// create new scenario based on parameters specification
//				SimulationScenario scenario = client.RegisterSimulationScenario(
//					simulationName, simulationBinariesPath, simulationParameters, executorPath, scenarioParams);
				
                // Get existing scenario object
				// SimulationScenario scenario = client.GetScenarioById("54293cce20a6f123c5000038");
				
//                Console.WriteLine("Got scenario with name: {0}, created at: {1}", scenario.Name, scenario.CreatedAt);

				// define few point of parameter space
//                points = new List<ValuesMap>() {
//					new ValuesMap() {
//	                    {"parameter1", 3.0},
//						{"parameter2", 4.0}
//					},
//					new ValuesMap() {
//						{"parameter1", 5.0},
//						{"parameter2", 7.0}
//					},
//					new ValuesMap() {
//						{"parameter1", 2.0},
//						{"parameter2", 9.0}
//					},
//					new ValuesMap() {
//						{"parameter1", 12.0},
//						{"parameter2", 2.0}
//					}
//				};

				// create new experiment based on scenario
//                Experiment experiment = scenario.CreateExperimentWithPoints(points, experimentParams);

				// get existing experiment
				Experiment experiment = client.GetExperimentById("5440526220a6f11a97000151");

                Console.WriteLine("Got experiment with ID: {0}", experiment.Id);

				// -- workers scheduling --
				/*
				 * 
				// will contain list of worker objects
				var jobs = new List<SimulationManager>();

				// schedule directly on Zeus PBS (preferred for Zeus)
				jobs.AddRange(experiment.ScheduleZeusJobs(1, "plgjliput", plgPass));

				// schedule on several PL-Grid Computin Engines using QosCosGrid
				var ces = new List<string> {PLGridCE.NOVA, PLGridCE.REEF};
				foreach (string ce in ces) {
					jobs.AddRange(experiment.SchedulePlGridJobs(ce, 1, "plgjliput", plgPass, plgPass));
				}

                foreach (var j in jobs) {
                    Console.WriteLine("Scheduled: {0} {1}", j.Id, j.State);
                }
				*/

				// Blocking method
                // experiment.WaitForDone();

				// using event to wait for experiment completion
				experiment.ExperimentCompleted += ShowResults;
				experiment.WatchingIntervalSecs = 3;
				experiment.StartWatching();

				// idle loop... remove if using WaitForDone!
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

