using System;
using System.Collections.Generic;

namespace Scalarm
{
	public interface IExperiment : IScalarmObject
	{
		string Id {get;}
		string Name { get; }
		string Description {get;}
		string SimulationId {get;}
		bool IsRunning {get;}
		int ReplicationLevel {get;}
		int TimeConstraintSec {get;}
		DateTime StartAt {get;}
		string UserId {get;}
		string SchedulingPolicy {get;}
		List<ExperimentInput.Category> InputSpecification { get; set; }
		int Size {get;}
		bool IsSupervised {get;}

		/// <summary>
		/// Get and save experiment binary package in .zip format.
		/// </summary>
		/// <param name="path">Local path to save results (.zip file will be created)</param>
		void GetBinaryResults(string path);

		IList<SimulationManager> ScheduleSimulationManagers(string infrastructure, 
			int count, IDictionary<string, object> parameters = null);

		IList<SimulationManager> ScheduleZeusJobs(int count, string plgridLogin, string plgridPassword);

		IList<SimulationManager> ScheduleZeusJobs(int count, IDictionary<string, object> parameters = null);

		IList<SimulationManager> SchedulePrivateMachineJobs(int count, PrivateMachineCredentials credentials);

		IList<SimulationManager> SchedulePrivateMachineJobs(int count, string credentialsId);

		/// <summary>
		///  Schedule jobs on PL-Grid for this experiment using PL-Grid UI login, password and Grid Certificate passphrase.
		/// </summary>
		/// <returns>The pl grid jobs.</returns>
		/// <param name="plgridCe">Target Computing Engine (cluster). Allowed values are stored in PLGridCE class. If null, "zeus.cyfronet.pl" is used.</param>
		/// <param name="count">How many jobs should be created (parallel computations).</param>
		IList<SimulationManager> SchedulePlGridJobs(string plgridCe, int count, string plgridLogin,
			string plgridPassword, string keyPassphrase);

		/// <summary>
		///  Schedule jobs on PL-Grid for this experiment using externally loaded PL-Grid Proxy Certificate string.
		/// NOTICE: if using ProxyCertClient, please use SchedulePlGridJobs(string plgridCe, int count) method!
		/// </summary>
		/// <returns>The pl grid jobs.</returns>
		/// <param name="plgridCe">Target Computing Engine (cluster). Allowed values are stored in PLGridCE class. If null, "zeus.cyfronet.pl" is used.</param>
		/// <param name="count">How many jobs should be created (parallel computations).</param>
		IList<SimulationManager> SchedulePlGridJobs(string plgridCe, int count, string plgridProxy);

		/// <summary>
		///  Schedule jobs on PL-Grid for this experiment using proxy certificate held by associated Client.
		///  Notice that this method can be used only with ProxyCertClient!
		/// </summary>
		/// <returns>The pl grid jobs.</returns>
		/// <param name="plgridCe">Target Computing Engine (cluster). Allowed values are stored in PLGridCE class. If null, "zeus.cyfronet.pl" is used.</param>
		/// <param name="count">How many jobs should be created (parallel computations).</param>
		IList<SimulationManager> SchedulePlGridJobs(string plgridCe, int count);

		ExperimentStatistics GetStatistics();

		bool IsDone();

		// TODO: check if there are workers running - if not - throw exception!
		/// <summary>
		///  Actively waits for experiment for completion. 
		/// </summary>
		void WaitForDone(int timeoutSecs=-1, int pollingIntervalSeconds=5);

		void StartWatching();

		void StopWatching();


		// TODO: parse json to resolve types?
		// <summary>
		//  Gets results in form od Dictionary: input parameters -> MoEs
		//  Input parameters and MoEs are in form of dictionaries: id -> value; both keys and values are string!
		// </summary>
		IList<SimulationParams> GetResults(Boolean fetchFailed = false);

		void FillSimulationParamsMap(IList<ValuesMap> results, IList<string> parametersIds);

		/// <summary>
		/// Gets simulation managers for this Experiment.
		/// </summary>
		/// <returns>All simulation managers associated with this Experiment.</returns>
		/// <param name="additionalParams">Additional query parameters.
		/// See additionalParams for Client.GetAllSimulationManagers for details (except for experiment_id).</param>
		IList<SimulationManager> GetSimulationManagers(IDictionary<string, object> additionalParams = null);
	}
}

