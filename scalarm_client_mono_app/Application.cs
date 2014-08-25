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
			
			var simulationName = "Hello simulation";
			var simulationBinariesPath = "/home/kliput/Programowanie/scalarm/inne/moja-symulacja-0/bin.zip";
			// TODO: as a string/json/?
			var simulationInputPath = "/home/kliput/Programowanie/scalarm/inne/moja-symulacja-0/input.json";
			var executorPath = "/home/kliput/Programowanie/scalarm/inne/moja-symulacja-0/executor.py";
			var simulationRegisterParams = new Dictionary<string, object>()
			{
				{"simulation_description", "This is a hello world test"},
				{"input_writer", "/home/kliput/Programowanie/scalarm/inne/moja-symulacja-0/input_writer.py"},
				// TODO: input writer id
				{"output_reader", "/home/kliput/Programowanie/scalarm/inne/moja-symulacja-0/output_reader.py"},
				// TODO: output reader id
				{"progress_monitor", "/home/kliput/Programowanie/scalarm/inne/moja-symulacja-0/progress_monitor.rb"},
				// TODO: progress monitor id
			};
			
			// TODO: use JSON serializer
			string experimentInput = @"
[ 
  { ""id"": ""main_category"", ""entities"": 
    [ 
      { ""id"": ""main_group"", ""parameters"": 
        [ 
          { ""id"": ""parameter1"", ""min"":""0"",""max"":""1000"",""parametrizationType"":""range"",""step"":""1"" },
          { ""id"": ""parameter2"", ""min"":""10"",""max"":""20"",""parametrizationType"":""range"",""step"":""2"" }
        ] 
      } 
    ] 
  }
]
";
			// TODO: use JSON serializer
			string experimentDoe = @"
[
    [ ""2k"",
      [ ""main_category___main_group___parameter1"",
        ""main_category___main_group___parameter2""
      ]
    ]
]
";
			
			// "[{"id":"main_category","label":"Main+category","entities":[{"id":"main_group","label":"Main+parameters","parameters":[{"id":"parameter1","label":"A","type":"integer","min":"0","max":"1000","parametrizationType":"range","step":"1"},{"id":"parameter2","label":"B","type":"integer","min":-100,"max":100,"parametrizationType":"value","value":"0.0"}]}]}]"
			
			var experimentParams = new Dictionary<string, object>()
			{
				{"experiment_name", "Some experiment"},
				{"experiment_description", "This is a dummy experiment"},
				{"doe", experimentDoe},
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
            } catch (ScalarmResourceException e) {
                Console.WriteLine("Error getting Scalarm resource: {0}", e.Resource.ErrorCode);
            } catch (FileNotFoundException e) {
                Console.WriteLine("Configuration file not found ({0})", e.FileName);
            }
		}

        static void Main3() {
            try {
                string configText = System.IO.File.ReadAllText("config.json");
                var config = JsonConvert.DeserializeAnonymousType(configText, new {base_url = "", login = "", password = ""});

                var client = new Client(config.base_url, config.login, config.password);
                SimulationScenario scenario = client.GetScenarioById("53eb031b4269a855fc000067");

                Console.WriteLine("{0} {1} {2}", scenario.Name, scenario.UserId, scenario.CreatedAt);
            } catch (InvalidResponseException e) {
                Console.WriteLine("Invalid response: {0}; {1}; {2};\n{3}", e.Response.Content, e.Response.ErrorMessage, e.GetBaseException(), e.Response.ErrorException);
            } catch (ScalarmResourceException e) {
                Console.WriteLine("Error getting Scalarm resource: {0}", e.Resource.ErrorCode);
            } catch (FileNotFoundException e) {
                Console.WriteLine("Configuration file not found ({0})", e.FileName);
            }
        }
	}
}

