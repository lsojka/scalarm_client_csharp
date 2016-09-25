using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

// ported
using System.Windows.Forms;
using System.Text;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using RestSharp.Deserializers;
using Scalarm;
using Scalarm.ExperimentInput;
using System.Diagnostics;


namespace scalarm_client_csharp_monitor.AppLogic
{
	public class Supervisor 
	{
		// FIELDS - REGULAR DATA

		ScalarmAppConfig config;
		public Client client;

		List<Experiment> experiments = new List<Experiment>();
		List<SimulationScenario> scenarios = new List<SimulationScenario>();
		ExperimentsListResult serverExperiments;
		List<ExperimentChecker> _reporters = new List<ExperimentChecker>();

		// FIELDS - CONCURRENCY

		public BlockingCollection<NurbsIntermediateResult> buffer1 = new BlockingCollection<NurbsIntermediateResult>();


		public class ScalarmAppConfig
		{
			public string base_url = null;
			public string login = null;
			public string password = null;
			public string plgrid_login = null;
			public string proxy_path = null;
			public string experiment_id = null;
		}
		
		// EVENTS 

		public delegate void FetchingExperimentsHandler(object source, FetchedExperimentsEventArgs e);
        public delegate void IntermediateResultUpdateHandler(object source, IntermediateResultUpdateEventArgs e);

		public event FetchingExperimentsHandler FetchingExperimentsEvent;
        public event IntermediateResultUpdateHandler IntermediateResultUpdateEvent;

		// METHODS - UTILITIES
		static private Random RAND = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

		static public int GetRandomNumber(int n)
		{
			return RAND.Next(n);
		}

        // http://stackoverflow.com/questions/5057567/
        public void Logger(String lines)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("D:\\MASTER\\log.txt", true);
            file.WriteLine(lines);

            file.Close();

        }

		// METHODS - SCALARM HANDLING
		public static ScalarmAppConfig ReadConfig(string path)
		{
			string configText = System.IO.File.ReadAllText(path);
			return JsonConvert.DeserializeObject<ScalarmAppConfig> (configText);

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
			//MessageBox.Show("login : " + config.login + "\n"+ "pass : " + config.password  + "plg login : " + config.plgrid_login);
		   

			// 2. Load scenarion
			// expendable data
			var randomNum = Supervisor.GetRandomNumber(9999);
			var baseScenarioPath = "example_scenario";

			// scenario definition

			var scenarioName = String.Format("SSRVE - monitored - {0}", randomNum);
			// returns {baseScenarioPath}/{0}"
			Func<string, string> scenarioPath = p => string.Format("{0}{1}{2}", baseScenarioPath, Path.DirectorySeparatorChar, p);
			var sscenarioBinariesPath = scenarioPath("bin.zip");
			var simulationInputDefinition = scenarioPath("input_definition.json");
			var executorPath = scenarioPath("executor.py");
			// open other files here
			var scenarioParams = new Dictionary<string, object>() { };
			scenarioParams.Add("input_writer", scenarioPath("input_writer.py"));
			scenarioParams.Add("output_reader", scenarioPath("output_reader.py"));
			scenarioParams.Add("progress_monitor", scenarioPath("progress_monitor.py"));
			
			try
			{
				// 3.2.
				// create scenario based and stuff it with parameter space
				SimulationScenario scenario = client.RegisterSimulationScenario(
					scenarioName, 
					sscenarioBinariesPath, 
					simulationInputDefinition,
					executorPath, 
					scenarioParams);
				
				MessageBox.Show("Got scenario with name: " + scenario.Name);

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

				var experimentParams = new Dictionary<string, object>()
				{
					{"experiment_name", String.Format("New experiment {0}", randomNum)},
					{"experiment_description", "This is a dummy experiment"},
					// {"doe", experimentDoe},
					{"execution_time_constraint", 3600}
				};
				Experiment experiment = scenario.CreateExperimentWithPoints(points, experimentParams);

				MessageBox.Show("Got experiment with ID: {0}", experiment.Id);
				//Console.WriteLine("Got experiment with ID: {0}", experiment.Id);

				// will contain list of worker objects
				List<SimulationManager> jobs = new List<SimulationManager>();

 
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

		public void getExperimentsFromServer()
		{
			//rezultat ¿¹dania wszystkich eksperymentów
			this.serverExperiments = client.GetAllExperimentIds();
			

			if (FetchingExperimentsEvent != null)
			{
				var dataObject = new FetchedExperimentsEventArgs(serverExperiments.running);
				//dataObject.runningExperiments = serverExperiments.running
				FetchingExperimentsEvent(this, dataObject);
			}
		}

		public void createClient()
		{
			config = Supervisor.ReadConfig("config.json");
			client = Supervisor.CreateClient(config);
		}

		// PIPELINE METHOD
		public void GetSingleResult(string _Id,
									BlockingCollection<NurbsIntermediateResult> output)
		{
            Stopwatch sW = new Stopwatch();
            sW.Start();
			// get response
			var nir = client.GetIntermediateExperimentResults(_Id);
			try
			{
				output.Add(nir);
                // TODO count time
				Application.DoEvents();
			}
			finally
			{
				output.CompleteAdding();
			}
            sW.Stop();
            Logger("IntermediateResult grab time = " + sW.Elapsed);
			// throwing an event to form

		}

		// GUI WRAPPER - monitoring tasks
		public void StartMonitoring(string _Id)
	   {
           IntermediateResultUpdateEventArgs eventArgs = new IntermediateResultUpdateEventArgs();
            
            // start multiple resultcollectors tasks (producers on separate threads)
			//var producer = Task.Factory.StartNew(() =>
			//{

                GetSingleResult(_Id, buffer1);
			//});
           
            // and then share with the results - throw event to GUI
            if (IntermediateResultUpdateEvent != null)
            {
                eventArgs.data = "xD";
                IntermediateResultUpdateEvent(this, eventArgs);
            }

            
		   
		    /*
		    var autoEvent = new AutoResetEvent(false);
		    var experimentChecker = new ExperimentChecker(5);
			
		    var collectorTimer = new System.Threading.Timer(
			    delegate { GetSingleResult(_Id, this.resStack); },
			    autoEvent,
			    1000,
			    5000);
		    collectorTimer.ex
		    // start the form
		    autoEvent.WaitOne();
		    collectorTimer.Dispose();
		    */
			

			// throw the data to the presenter class/form
			// OR
			// gui can call method returing all neccessary data to display
		}
	}
}

public class FetchedExperimentsEventArgs : EventArgs
{
	public List<string> runningExperiments { get; set; }

	public FetchedExperimentsEventArgs(List<string> _e)
	{
		runningExperiments = new List<string>(_e);
	}
}

// obiekt danych
public class IntermediateResultUpdateEventArgs : EventArgs
{
    public string data { get; set; }
}

