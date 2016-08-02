using Scalarm.ExperimentInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Dynamic.Utils;

namespace Scalarm
{
    public class ExampleFullRegister
    {
        public void Run()
        {
            // Idle loop looking for someone to clik on/activate element.
            // TODO - Google it

            // 1. Get json, passes, scenario
            // TODO - Method that extracts from file and stores for use
			var config = Application.ReadConfig ("config.json");
            var client = Application.CreateClient(config);

			var usingProxyClient = (client is ProxyCertClient);

			string plgLogin = usingProxyClient ? "" : (String.IsNullOrEmpty(config.plgrid_login)
			                                           ? Application.ReadString("Enter PL-Grid login:") : config.plgrid_login);

			string plgPass = usingProxyClient ? "" : Application.ReadPassword("Enter PL-Grid UI password:");
			string plgKeyPass = usingProxyClient ? "" : Application.ReadPassword("Enter PL-Grid Certificate / current scalarm password:");

            
            var randomNum = Application.GetRandomNumber(999);
            var simulationName = String.Format("New simulation {0}", randomNum);
		    
            var baseScenarioPath = "example_scenario";

			Func<string, string> scenarioPath = p => string.Format("{0}{1}{2}", baseScenarioPath, Path.DirectorySeparatorChar, p);
            var simulationBinariesPath = scenarioPath("bin.zip");
            var executorPath = scenarioPath("executor.py");

            // 2. Load params.  scenario & experiment
            var scenarioParams = new Dictionary<string, object>() { };

            // injected scenario data
            var experimentParams = new Dictionary<string, object>()
			{
	            {"experiment_name", String.Format("New experiment {0}", randomNum)},
				{"experiment_description", "This is a dummy experiment"},
				// {"doe", experimentDoe},
				{"execution_time_constraint", 3600}
			};


            /* 3.1. Parameter specification
             *
             * 
             *    Parameters have to be read from supplied file - hardcoded and loaded from disk or GUI loader
             *    TODO - Verifier - parser, or just lean on Scalarm?
             */
             
            try
            {
                // TODO: below method with executor id instead of path overload
                
                // define input parameters specification
                var simulationParameters = new List<ExperimentInput.Parameter>() {
                    new Parameter("paramteer1", "Param 1") {
                        // this is initialization for a parameter
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

                // 3.2.
                // create scenario based and stuff it with parameter space
                SimulationScenario scenario = client.RegisterSimulationScenario(
                    simulationName, simulationBinariesPath, simulationParameters, executorPath, scenarioParams);
                
                Console.WriteLine("Got scenario with name: {0}, created at: {1}", scenario.Name, scenario.CreatedAt);

                List<ValuesMap> points;

                // define few point of parameter space
                points = new List<ValuesMap>() {
                	new ValuesMap() {
                	    {"parameter1", 1.5},
                		{"parameter2", 3}
                	},
                	new ValuesMap() {
                		{"parameter1", 5.0},
                		{"parameter2", 7.0}
                	},
                	new ValuesMap() {
                		{"parameter1", 2.0},
                		{"parameter2", 9.0}
                	},
                	new ValuesMap() {
                		{"parameter1", 12.0},
                		{"parameter2", 2.0}
                	}
                };

                // define more point of parameter space
                for (int i = 0; i < 10; ++i)
                {
                    points.Add(new ValuesMap() {
					    {"parameter1", (float) Application.GetRandomNumber(1000)},
					    {"parameter2", (float) Application.GetRandomNumber(1000)}
				    });
                }

                // 4. Create new experiment based on scenario
				Experiment experiment = scenario.CreateExperimentWithPoints(points, experimentParams);

                Console.WriteLine("Got experiment with ID: {0}", experiment.Id);

                // will contain list of worker objects
                List<SimulationManager> jobs = new List<SimulationManager>();

                // 5. -- Workers scheduling on PL-Grid --
                // Not for now.
                /*
                // TODO: parametrize! - legacy todo

                // schedule directly on Zeus PBS (preferred for Zeus)

				if (usingProxyClient) {
					jobs.AddRange(experiment.ScheduleZeusJobs(1));
				} else {
					jobs.AddRange(experiment.ScheduleZeusJobs(1, plgridLogin: plgLogin, plgridPassword: plgPass));
				}

                // schedule on several PL-Grid Computing Engines using QosCosGrid: Nova and Reef clusters
                var ces = new List<string> {PLGridCE.NOVA, PLGridCE.REEF};
                foreach (string ce in ces) {
					if (usingProxyClient) {
						jobs.AddRange(experiment.SchedulePlGridJobs(ce, 1));
					} else {
						jobs.AddRange(experiment.SchedulePlGridJobs(ce, 1, plgridLogin: plgLogin, plgridPassword: plgPass, keyPassphrase: plgKeyPass));
					}
                }

                foreach (var j in jobs) {
                    Console.WriteLine("Scheduled: {0} {1}", j.Id, j.State);
                }
                
				// Let's stop last started SimulationManager
				SimulationManager lastSim = jobs.Last();
				Console.WriteLine("Stopping Simulation Manager with ID: {0}", lastSim.Id);
				lastSim.Stop();

				// and check current simulation managers state
				IList<SimulationManager> currentJobs = experiment.GetSimulationManagers();
				Console.WriteLine("Current Simulation Managers:");
				foreach (var j in jobs) {
					Console.WriteLine("{0} -> {1}", j.Id, j.State);
				}

                // -- workers scheduling on servers -- TODO

                // Blocking method
                // experiment.WaitForDone();

                // using event to wait for experiment completion
                experiment.ExperimentCompleted += Application.ShowResults;
				experiment.NoResources += Application.HandleNoResources;
                experiment.WatchingIntervalSecs = 4;
                experiment.StartWatching();

                
                // idle loop... remove if using WaitForDone!
                while (Application.ShouldWait)
                {
                    Console.WriteLine("DEBUG: waiting...");
					currentJobs = experiment.GetSimulationManagers();
					Console.WriteLine("State of Simulation Managers: {0}", string.Join(", ", jobs.Select(i => i.State)));
                    Thread.Sleep(1000);
                }
                */
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
