using Scalarm.ExperimentInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Dynamic.Utils;

namespace Scalarm
{
	public class ExampleFullRegister
	{
		public static void Run()
		{
			var config = Application.ReadConfig ("config.json");
			var client = Application.CreateClient(config);

			var usingProxyClient = (client is ProxyCertClient);

			string plgLogin = usingProxyClient ? "" : (String.IsNullOrEmpty(config.plgrid_login)
				? Application.ReadString("Enter PL-Grid login:") : config.plgrid_login);

			string plgPass = usingProxyClient ? "" : Application.ReadPassword("Enter PL-Grid UI password:");
			string plgKeyPass = usingProxyClient ? "" : Application.ReadPassword("Enter PL-Grid Certificate password:");

			var randomNum = Application.GetRandomNumber(1000);

			//            var simulationName = String.Format("New simulation {0}", randomNum);
			//			var baseScenarioPath = "example_scenario";
			//
			//			Func<string, string> scenarioPath = p => string.Format("{0}{1}{2}", baseScenarioPath, Path.DirectorySeparatorChar, p);
			//            var simulationBinariesPath = scenarioPath("bin.zip");
			//            var executorPath = scenarioPath("executor.py");
			//
			//            var scenarioParams = new Dictionary<string, object>() { };

			//            var experimentParams = new Dictionary<string, object>()
			//		{
			//            {"experiment_name", String.Format("New experiment {0}", randomNum)},
			//			{"experiment_description", "This is a dummy experiment"},
			//			// {"doe", experimentDoe},
			//			{"execution_time_constraint", 3600}
			//		};

			try
			{
				// --------------------dodanie nowego scenariusza symulacji

				string simulationName = "RectangleArea test " + randomNum.ToString();

				string baseScenarioPath = "daniel_scenario";
				Func<string, string> scenarioPath = p => string.Format("{0}{1}{2}", baseScenarioPath, Path.DirectorySeparatorChar, p);
				var simulationBinariesPath = scenarioPath("bin.zip");
				var executorPath = scenarioPath("executor.py");
				var inputWriterPath = scenarioPath("input_writer.py");
				var outputReaderPath = scenarioPath("output_reader.py");

				var fe = File.Exists(simulationBinariesPath);
				fe = File.Exists(executorPath);

				var simulationParameters = new List<Scalarm.ExperimentInput.Parameter>() {
					new Scalarm.ExperimentInput.Parameter("a", "A") {
						ParametrizationType = Scalarm.ExperimentInput.ParametrizationType.RANGE,
						Type = Scalarm.ExperimentInput.Type.FLOAT,
						Min = 0.00001f, Max = 10000
					},
					new Scalarm.ExperimentInput.Parameter("b", "B") {
						ParametrizationType = Scalarm.ExperimentInput.ParametrizationType.RANGE,
						Type = Scalarm.ExperimentInput.Type.FLOAT,
						Min = 0.00001f, Max = 10000
					}
				};



				// --------------------dodanie nowego scenariusza symulacji

				SimulationScenario symScenario = client.RegisterSimulationScenario(
					simulationName, simulationBinariesPath, simulationParameters, executorPath, new Dictionary<string, object> {
						{"input_writer", inputWriterPath},
						{"output_reader", outputReaderPath}
					});

				// --------------------dodanie eksperymenu

				//Scalarm.SimulationScenario symScenario = client.GetScenarioById("55d4718a4269a80fbb015826");

				List<Scalarm.ValuesMap> points = new List<ValuesMap>() {
					new Scalarm.ValuesMap() {
						{"a", 1},
						{"b", 2}
					},
					new Scalarm.ValuesMap() {
						{"a", 1.5},
						{"b", 3}
					},
					new Scalarm.ValuesMap() {
						{"a", 2.5},
						{"b", 3}
					},
					new Scalarm.ValuesMap() {
						{"a", 3.5},
						{"b", 3}
					},
				};

				var rnd = new Random();

				for (int i=0; i<1000; ++i) {
					points.Add(new ValuesMap {
						{"a", rnd.NextDouble()},
						{"b", rnd.NextDouble()}
					});
				}


				Dictionary<string, object> experimentParams = new Dictionary<string, object>() {
					{"experiment_name", String.Format("New experiment {0}", new Random().Next())},
					{"experiment_description", "test experiment of CalculateRectangle scenario"},
					{"execution_time_constraint", 5}
				};

				Scalarm.Experiment experiment = symScenario.CreateExperimentWithPoints(points, experimentParams);


				// --------------------uruchomienie eksperymentu

				// "55d5a573fc3ff95813000059";
				var experimentId = experiment.Id;

				Experiment exp = client.GetExperimentById<Scalarm.Experiment>(experimentId);

				// will contain list of worker objects
				List<SimulationManager> jobs = new List<SimulationManager>();
				//				var reqParams = new Dictionary<string, object> {
				//					{"time_limit", "60"},
				//				};
				//               jobs.AddRange(exp.ScheduleSimulationManagers("qsub", 1, reqParams));

				jobs.AddRange(exp.ScheduleZeusJobs(1));

				// --------------------uruchomienie eksperymentu



				// and check current simulation managers state
				IList<SimulationManager> currentJobs = exp.GetSimulationManagers();
				Console.WriteLine("Current Simulation Managers:");
				foreach (var j in currentJobs)
				{
					Console.WriteLine("{0} -> {1}", j.Id, j.State);
				}


				// -------------------------------obliczenia na Scalarmie
			}
			catch (RegisterSimulationScenarioException e)
			{
				Console.WriteLine("Registering simulation scenario failed: " + e);
			}
			catch (CreateScenarioException e)
			{
				Console.WriteLine("Creating experiment failed: " + e);
			}
			catch (InvalidResponseException e)
			{
				Console.WriteLine("Invalid response: {0};\n\n{1};\n\n{2}", e.Response.Content, e.Response.ErrorMessage, e.Response.ErrorException);
			}
			catch (ScalarmResourceException<SimulationScenario> e)
			{
				Console.WriteLine("Error getting Scalarm SimulationScenario resource: {0}", e.Resource.ErrorCode);
			}
		}
	}
}
