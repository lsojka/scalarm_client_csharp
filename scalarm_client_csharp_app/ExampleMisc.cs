﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scalarm
{
    class ExampleMisc
    {
        public static void Run()
        {
            // TODO

            //string plgPass = Application.ReadPassword();

            //var client = Application.CreateClient("config.json");

            //var randomNum = Application.GetRandomNumber(1000);

            //var simulationName = String.Format("New simulation {0}", randomNum);
            //var baseScenarioPath = "/home/kliput/Programowanie/scalarm/inne/moja-symulacja-double/";

            //Func<string, string> scenarioPath = p => string.Format("{0}/{1}", baseScenarioPath, p);
            //var simulationBinariesPath = scenarioPath("bin.zip");
            //var executorPath = scenarioPath("executor.py");

            //var scenarioParams = new Dictionary<string, object>() { };

            //var experimentParams = new Dictionary<string, object>()
            //{
            //    {"experiment_name", String.Format("New experiment {0}", randomNum)},
            //    {"experiment_description", "This is a dummy experiment"},
            //    // {"doe", experimentDoe},
            //    {"execution_time_constraint", 3600}
            //};

            //try
            //{
            //    // TODO: below method with executor id instead of path overload

            //    // define input parameters specification
            //    //                var simulationParameters = new List<ExperimentInput.Parameter>() {
            //    //                    new Parameter("parameter1", "Param 1") {
            //    //                        ParametrizationType = ExperimentInput.ParametrizationType.RANGE,
            //    //                        Type = ExperimentInput.Type.FLOAT,
            //    //                        Min = 0, Max = 1000
            //    //                    },
            //    //					new Parameter("parameter2", "Param 2") {
            //    //						ParametrizationType = ExperimentInput.ParametrizationType.RANGE,
            //    //						Type = ExperimentInput.Type.FLOAT,
            //    //						Min = -100, Max = 100
            //    //					}
            //    //                };

            //    // create new scenario based on parameters specification
            //    //				SimulationScenario scenario = client.RegisterSimulationScenario(
            //    //					simulationName, simulationBinariesPath, simulationParameters, executorPath, scenarioParams);

            //    // Get existing scenario object
            //    SimulationScenario scenario = client.GetScenarioById("5440526220a6f11a9700014f");

            //    //                Console.WriteLine("Got scenario with name: {0}, created at: {1}", scenario.Name, scenario.CreatedAt);

            //    var points = new List<ValuesMap>();

            //    // define few point of parameter space
            //    //                points = new List<ValuesMap>() {
            //    //					new ValuesMap() {
            //    //	                    {"parameter1", 3.0},
            //    //						{"parameter2", 4.0}
            //    //					},
            //    //					new ValuesMap() {
            //    //						{"parameter1", 5.0},
            //    //						{"parameter2", 7.0}
            //    //					},
            //    //					new ValuesMap() {
            //    //						{"parameter1", 2.0},
            //    //						{"parameter2", 9.0}
            //    //					},
            //    //					new ValuesMap() {
            //    //						{"parameter1", 12.0},
            //    //						{"parameter2", 2.0}
            //    //					}
            //    //				};

            //    // define more point of parameter space
            //    for (int i = 0; i < 1000; ++i)
            //    {
            //        points.Add(new ValuesMap() {
            //            {"parameter1", (float) Application.GetRandomNumber(1000)},
            //            {"parameter2", (float) Application.GetRandomNumber(1000)}
            //        });
            //    }

            //    // create new experiment based on scenario
            //    Experiment experiment = scenario.CreateExperimentWithPoints(points, experimentParams);

            //    // get existing experiment
            //    // Experiment experiment = client.GetExperimentById("5440526220a6f11a97000151");

            //    Console.WriteLine("Got experiment with ID: {0}", experiment.Id);

            //    // will contain list of worker objects
            //    var jobs = new List<SimulationManager>();

            //    // -- workers scheduling on PL-Grid --
            //    /*
            //    // schedule directly on Zeus PBS (preferred for Zeus)
            //    jobs.AddRange(experiment.ScheduleZeusJobs(1, "plgjliput", plgPass));

            //    // schedule on several PL-Grid Computin Engines using QosCosGrid
            //    var ces = new List<string> {PLGridCE.NOVA, PLGridCE.REEF};
            //    foreach (string ce in ces) {
            //        jobs.AddRange(experiment.SchedulePlGridJobs(ce, 1, "plgjliput", plgPass, plgPass));
            //    }

            //    foreach (var j in jobs) {
            //        Console.WriteLine("Scheduled: {0} {1}", j.Id, j.State);
            //    }
            //    */

            //    // -- workers scheduling on servers --

            //    // Add private machine credentials
            //    //var creds = client.AddPrivateMachineCredentials("localhost", "user1", "pass");

            //    // Find previously registered private machine credentials
            //    var serverCredsList = client.GetPrivateMachineCredentials("localhost", "testowy");

            //    jobs.AddRange(experiment.SchedulePrivateMachineJobs(2, serverCredsList[0]));

            //    // Blocking method
            //    // experiment.WaitForDone();

            //    // using event to wait for experiment completion
            //    experiment.ExperimentCompleted += Application.ShowResults;
            //    experiment.WatchingIntervalSecs = 3;
            //    experiment.StartWatching();

            //    // idle loop... remove if using WaitForDone!
            //    while (Application.ShouldWait)
            //    {
            //        Console.WriteLine("DEBUG: waiting...");
            //        Thread.Sleep(1000);
            //    }


            //}
            //catch (RegisterSimulationScenarioException e)
            //{
            //    Console.WriteLine("Registering simulation scenario failed: " + e);
            //}
            //catch (CreateScenarioException e)
            //{
            //    Console.WriteLine("Creating experiment failed: " + e);
            //}
            //catch (InvalidResponseException e)
            //{
            //    Console.WriteLine("Invalid response: {0};\n\n{1};\n\n{2}", e.Response.Content, e.Response.ErrorMessage, e.Response.ErrorException);
            //}
            //catch (ScalarmResourceException<SimulationScenario> e)
            //{
            //    Console.WriteLine("Error getting Scalarm SimulationScenario resource: {0}", e.Resource.ErrorCode);
            //}
            //catch (FileNotFoundException e)
            //{
            //    Console.WriteLine("Configuration file not found ({0})", e.FileName);
            //}
        }
    }
}
