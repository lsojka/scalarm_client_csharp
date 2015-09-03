using System;
using Scalarm;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Scalarm
{
	public abstract class AbstractTestGetSimError
	{
		public abstract void scheduleWorkers(Experiment experiment);

		public void Run()
		{
			var config = Application.ReadConfig ("config.json");
			var client = Application.CreateClient(config);

			if (!(client is ProxyCertClient)) {
				throw new ApplicationException("This test supports only proxy cert client");
			}

			var randomNum = Application.GetRandomNumber(1000);
			var simulationName = "TestGetSimError_" + randomNum;
			var experimentName = "TestGetSimError_exp_" + randomNum;

			var simDir = "test_scenario_error_executor";
			var executorPath = string.Format("{0}{1}{2}", simDir, Path.DirectorySeparatorChar, "executor.sh");
			var binPath = string.Format("{0}{1}{2}", simDir, Path.DirectorySeparatorChar, "hello.zip");

			var simulationParameters = new List<Scalarm.ExperimentInput.Parameter>() {
				new Scalarm.ExperimentInput.Parameter("a", "A") {
					ParametrizationType = Scalarm.ExperimentInput.ParametrizationType.RANGE,
					Type = Scalarm.ExperimentInput.Type.FLOAT,
					Min = 0.1f, Max = 10
				},
			};

			SimulationScenario simScenario = client.RegisterSimulationScenario(
				simulationName, binPath, simulationParameters, executorPath);

			List<Scalarm.ValuesMap> points = new List<ValuesMap>();

			Random rnd = new Random();
			for (int i=0; i<5; ++i) {
				points.Add(new ValuesMap {
					{"a", rnd.NextDouble()}
				});
			}

			Dictionary<string, object> experimentParams = new Dictionary<string, object>() {
				{ "experiment_name", experimentName },
				{ "execution_time_constraint", 5 }
			};

			var experiment = simScenario.CreateExperimentWithPoints(points, experimentParams);

			scheduleWorkers(experiment);

			try {
				experiment.WaitForDone(5*60*1000, 10);
			} catch (NoActiveSimulationManagersException) {
				IList<SimulationManager> workers = experiment.GetSimulationManagers();
				if (workers.Count != 1) {
					throw new ApplicationException("Invalid workers count: " + workers.Count);
				}
				var sim = workers.First();
				// waiting for log fetch
				for (int i = 1; i < 30; ++i) {
					if (String.IsNullOrEmpty(sim.ErrorDetails)) {
						Console.WriteLine("No error log fetched yet, waiting...");
						Thread.Sleep(1000);
					} else {
						break;
					}
				}

				Console.WriteLine("Simulation Manager failed with error/log:");
				Console.WriteLine(sim.Error);
				Console.WriteLine(sim.ErrorDetails);
			}
		}
	}
}

