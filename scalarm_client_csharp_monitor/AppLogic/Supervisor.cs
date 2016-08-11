using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

// ported
using System.Windows.Forms;
using System.Text;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using RestSharp.Deserializers;
using Scalarm;
using Scalarm.ExperimentInput;


namespace AppLogic
{
	public class Supervisor
	{
        // lots of fields!
        ScalarmAppConfig config;
        Client client;
        Scalarm.ExperimentInput e;
        // methods
		public static bool ShouldWait = true;

		// private static List<ValuesMap> points = new List<ValuesMap>();
		static private Random RAND = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

		static public int GetRandomNumber(int n)
		{
			return RAND.Next(n);
		}


        // ------------------------
		public class ScalarmAppConfig
		{
			public string base_url = null;
			public string login = null;
			public string password = null;
			public string plgrid_login = null;
			public string proxy_path = null;
			public string experiment_id = null;
		}

		public static ScalarmAppConfig ReadConfig(string path)
		{
			string configText = System.IO.File.ReadAllText(path);
			return JsonConvert.DeserializeObject<ScalarmAppConfig> (configText);
            // DEBUG

		}

        public static Client CreateClient(ScalarmAppConfig config)
        {
			if (String.IsNullOrEmpty (config.proxy_path)) {
				return new BasicAuthClient (config.base_url, config.login, config.password);
			} else {
				return new ProxyCertClient (config.base_url, new FileStream (config.proxy_path, FileMode.Open));
			}					
        }

        public void readConfig()
        {
            // register - first, with hardcoded data
            // then push to verver
            // lives only a click
            // 1. Load credentials
            // no try/catch as static metod does it safe
            config = Supervisor.ReadConfig("config.json");
            client = Supervisor.CreateClient(config);

            /*
            MessageBox.Show("login : " + config.login + "\n"
                            + "pass : " + config.password  
                            + "plg login : " + config.plgrid_login);
            */
            var randomNum = Supervisor.GetRandomNumber(9999);
            //var simulationName = String.Format("Simulation {0}", Supervisor.GetRandomNumber(9999));
            var simulationName = String.Format("Simulation {0}", randomNum);
            var baseScenarioPath = "example_scenario";
            // anonymous method that returns "example_scenario/p" path
            Func<string, string> scenarioPath = p => string.Format("{0}{1}{2}", baseScenarioPath, Path.DirectorySeparatorChar, p);
            var simulationBinariesPath = scenarioPath("bin.zip");
            var executorPath = scenarioPath("executor.py");

            var experimentParams = new Dictionary<string, object>()
            {
                {"experiment_name", String.Format("New experiment {0}", randomNum)},
				{"experiment_description", "This is a dummy experiment"},
				// {"doe", experimentDoe},
				{"execution_time_constraint", 3600}
            };
            // tu idzie TRY
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
                points = new List<ValuesMap>(){ 
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
					    {"parameter1", (float) Supervisor.GetRandomNumber(1000)},
					    {"parameter2", (float) Supervisor.GetRandomNumber(1000)}
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

