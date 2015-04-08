using Scalarm.ExperimentInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scalarm
{
    public class ExampleSimple
    {
        public static void Run()
        {
			var config = Application.ReadConfig ("config.json");
            var client = Application.CreateClient(config);

//			var usingProxyClient = (client is ProxyCertClient);

//			string plgLogin = usingProxyClient ? "" : (String.IsNullOrEmpty(config.plgrid_login)
//			                                           ? Application.ReadString("Enter PL-Grid login:") : config.plgrid_login);

//			string plgPass = usingProxyClient ? "" : Application.ReadPassword("Enter PL-Grid UI password:");
//			string plgKeyPass = usingProxyClient ? "" : Application.ReadPassword("Enter PL-Grid Certificate password:");

//            var randomNum = Application.GetRandomNumber(1000);

			try
            {
                // create new experiment based on scenario
                Experiment experiment = client.GetExperimentById(config.experiment_id);

                Console.WriteLine("Got experiment with ID: {0}", experiment.Id);

				experiment.ScheduleSimulationManagers("dummy", 3);

                // will contain list of worker objects
                IList<SimulationManager> workers = experiment.GetActiveSimulationManagers();

				Console.WriteLine("Got {0} active simulation managers", workers.Count);

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
                    Thread.Sleep(1000);
                }


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
