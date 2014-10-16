using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;
using System.Threading;
using System.Linq;
using System.Collections;
using System.ComponentModel;

namespace Scalarm
{	
	public delegate void ExperimentCompletedEventHandler(object sender, Experiment exp);

	public class Experiment : ScalarmObject
	{
		public event ExperimentCompletedEventHandler ExperimentCompleted;

		private int _watchingIntervalMillis = 5000;
		public int WatchingIntervalSecs {
			get {
				return _watchingIntervalMillis;
			}
			set {
				_watchingIntervalMillis = value * 1000;
			}
		}

        // TODO: make full experiment model

        public string ExperimentId {get; private set;}

		// TODO: it should be retrieved with experiment data from controller
		public List<Category> InputSpecification { get; set; }

        public Experiment(string experimentId, Client client) : base(client)
        {
            ExperimentId = experimentId;
        }

		protected void OnExperimentCompleted(EventArgs e)
		{
			// TODO not this this
			if (ExperimentCompleted != null) ExperimentCompleted(this, this);
		}

        public List<SimulationManager> ScheduleSimulationManagers(string infrastructure, int count, Dictionary<string, object> parameters) {
            return Client.ScheduleSimulationManagers(ExperimentId, infrastructure, count, parameters);
        }

        public List<SimulationManager> ScheduleZeusJobs(int count, string plgridLogin, string plgridPassword)
        {
			var reqParams = new Dictionary<string, object> {
				{"time_limit", "60"}
			};

			if (plgridLogin != null) {
				if (plgridPassword == null) {
					new ArgumentNullException ("PL-Grid password must not be null");
				}
				reqParams ["plgrid_login"] = plgridLogin;
				reqParams ["plgrid_password"] = plgridPassword;
				reqParams ["onsite_monitoring"] = true;
			}

            return ScheduleSimulationManagers("qsub", count, reqParams);
        }

		public List<SimulationManager> SchedulePlGridJobs(int count, string plgridLogin, string plgridPassword, string keyPassphrase) {
			return SchedulePlGridJobs(null, count, plgridLogin, plgridPassword, keyPassphrase);
		}

		public List<SimulationManager> SchedulePlGridJobs(string plgridCe, int count, string plgridLogin, string plgridPassword, string keyPassphrase)
		{
			var reqParams = new Dictionary<string, object> {
				{"time_limit", "60"}
			};

			reqParams ["plgrid_host"] = (plgridCe != null ? plgridCe : PLGridCE.ZEUS);

			if (plgridLogin != null) {
				if (plgridPassword == null || keyPassphrase == null) {
					new ArgumentNullException ("PL-Grid password and private key passphrase must not be null");
				}
				reqParams ["plgrid_login"] = plgridLogin;
				reqParams ["plgrid_password"] = plgridPassword;
				reqParams ["key_passphrase"] = keyPassphrase;
				reqParams ["onsite_monitoring"] = true;
			}

			return ScheduleSimulationManagers("qcg", count, reqParams);
		}

        public ExperimentStatistics GetStatistics()
        {
            return Client.GetExperimentStatistics(ExperimentId);
        }

        public bool IsDone()
        {
            var stats = GetStatistics();
			Console.WriteLine("DEBUG: exp stats: " + stats.ToString());
            return stats.All == stats.Done;
        }

        // TODO: check if there are workers running - if not - throw exception!
        /// <summary>
        ///  Actively waits for experiment for completion. 
        /// </summary>
        public void WaitForDone(int timeoutSecs=-1, int pollingIntervalSeconds=5)
        {
            var startTime = DateTime.UtcNow;

            while (timeoutSecs <= 0 || (DateTime.UtcNow - startTime).TotalSeconds < timeoutSecs) {
                if (IsDone()) {
                    return;
                }
                Thread.Sleep(pollingIntervalSeconds*1000);
            }
            throw new TimeoutException();
    	}

		private BackgroundWorker _worker;

		public void StartWatching()
		{
			if (_worker == null) {
				_worker = new BackgroundWorker();
				_worker.WorkerSupportsCancellation = true;
				_worker.WorkerReportsProgress = false;
				_worker.DoWork += _watchCompletion;
				_worker.RunWorkerCompleted += _workerCompleted;
			}

			if (!_worker.IsBusy) {
				_worker.RunWorkerAsync();
			}
		}

		public void StopWatching()
		{
			if (_worker != null) {
				_worker.CancelAsync();
			}
		}

		private void _watchCompletion(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker worker = sender as BackgroundWorker;

			Console.WriteLine("Starting experiment watching thread");
			while (!worker.CancellationPending && !IsDone()) {
				Thread.Sleep(_watchingIntervalMillis);
			}

			if (worker.CancellationPending) {
				e.Cancel = true;
			}
		}

		private void _workerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (!e.Cancelled) {
				if (e.Error == null) {
					OnExperimentCompleted(EventArgs.Empty);
				} else {
					throw e.Error;
				}
			}
		}

		// TODO: parse json to resolve types?
		// <summary>
		//  Gets results in form od Dictionary: input parameters -> MoEs
		//  Input parameters and MoEs are in form of dictionaries: id -> value; both keys and values are string!
		// </summary>
		public IDictionary<ValuesMap, ValuesMap> GetResults()
		{
			var results = Client.GetExperimentResults(ExperimentId);
			var parametersIds = InputDefinition.ParametersIdsForCategories(InputSpecification);

			return SplitParametersAndResults(ConvertTypes(results), parametersIds);
		}

		// TODO: can modify results, use with caution
		public static IList<ValuesMap> ConvertTypes(IList<ValuesMap> results)
		{
			var convertedResults = new List<ValuesMap>();
			foreach (var item in results) {
				convertedResults.Add(item);
			}

			foreach (var record in convertedResults) {
				foreach (var singleResult in record) {
					// TODO: check with string values - probably there will bo problem with deserializing because lack of ""
					record [singleResult.Key] = JsonConvert.DeserializeObject(singleResult.Value.ToString());
				}
			}

			return convertedResults;
		}

		public static IDictionary<ValuesMap, ValuesMap> SplitParametersAndResults(IList<ValuesMap> results, IList<string> parametersIds)
		{
			var finalDict = new Dictionary<ValuesMap, ValuesMap>();

			foreach (var result in results) {
				var resultDict = result.ShallowCopy();

				var paramsDict = new ValuesMap();

				foreach (string id in parametersIds) {
					if (resultDict.ContainsKey(id)) {
						paramsDict.Add(id, resultDict[id]);
						resultDict.Remove(id);
					}
				}
				finalDict.Add(paramsDict, resultDict);
			}

			return finalDict;
		}

		// TODO: this should be method for "Results" object?
		public ValuesMap GetSingleResult(ValuesMap point)
		{
			var results = GetResults();
			return results[point];
		}
	}

}

