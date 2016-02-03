using System;
using System.Collections.Generic;

namespace Scalarm
{
	/// <summary>
	/// Mock of Scalarm.Experiment for testing purposes.
	/// </summary>
	public class FakeSupervisedExperiment : ISupervisedExperiment
	{
		public List<Scalarm.ValuesMap> StoredPoints { get; set; }
		public Random Random { get; }

		public FakeSupervisedExperiment ()
		{
			StoredPoints = new List<Scalarm.ValuesMap> ();
			Random = new Random();

			Id = "FakeExperiment";
		}

		public Scalarm.Client Client { get; set; }

		public string Id {get;}
		public string Name { get; }
		public string Description {get;}
		public string SimulationId {get;}
		public bool IsRunning {get;}
		public int ReplicationLevel {get;}
		public int TimeConstraintSec {get;}
		public DateTime StartAt {get;}
		public string UserId {get;}
		public string SchedulingPolicy {get;}
		public List<ExperimentInput.Category> InputSpecification { get; set; }
		public int Size {get;}
		public bool IsSupervised {get;}


		public void GetBinaryResults(string path) {
			throw new NotImplementedException();
		}


		/// <summary>
		/// Not implemented, because in mock this is unavailable.
		/// </summary>
		/// <returns>The simulation managers.</returns>
		/// <param name="infrastructure">Infrastructure.</param>
		/// <param name="count">Count.</param>
		/// <param name="parameters">Parameters.</param>
		public IList<Scalarm.SimulationManager> ScheduleSimulationManagers(string infrastructure,
			int count, IDictionary<string, object> parameters = null) {

			throw new NotImplementedException();
		}

		public Scalarm.ExperimentStatistics GetStatistics() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Do exactly nothing. All parameters are fake.
		/// </summary>
		public void WaitForDone(int timeoutSecs=-1, int pollingIntervalSeconds=5) {}

		/// <summary>
		/// Return fake results for registered points.
		/// Currently only one MoE will be returned per result: "moe" -> double.
		/// Values are in range: 0.0 - 1.0
		/// </summary>
		/// <returns>The results.</returns>
		/// <param name="fetchFailed">Fake parameter</param>
		public IList<Scalarm.SimulationParams> GetResults(Boolean fetchFailed = false) {
			var results = new List<Scalarm.SimulationParams>();
			foreach (Scalarm.ValuesMap point in StoredPoints) {
				var output = new Scalarm.ValuesMap {
					{"moe", Random.NextDouble()}
				};
				var sp = new Scalarm.SimulationParams(point, output);
				results.Add(sp);
			}
			return results;
		}

		/// <summary>
		/// Fake function. Throws NotImplementedException.
		/// </summary>
		public IList<Scalarm.SimulationManager> GetSimulationManagers(IDictionary<string, object> additionalParams = null) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Write results to Console.
		/// </summary>
		/// <param name="results">Results.</param>
		/// <param name="success">If set to <c>true</c> success.</param>
		/// <param name="errorReason">Error reason.</param>
		public void MarkAsComplete(string results, bool success = true, string errorReason = null) {
			Console.WriteLine("EXPERIMENT MOCK mark as complete\n" +
				"Results:\n" + results + "\n" +
				"Success? " + success + "\n" +
				"Error reason\n: " + errorReason + "\n"
			);

		}

		/// <summary>
		/// Register point in this instance to use them for results "generation" in GetResults
		/// </summary>
		/// <param name="point">Point.</param>
		public void SchedulePoint(Scalarm.ValuesMap point)
		{
			// TODO: register points in instance to use them in GetResults
			StoredPoints.Add(point);
		}

		public void SchedulePoints(IEnumerable<Scalarm.ValuesMap> points)
		{
			foreach (Scalarm.ValuesMap point in points) {
				SchedulePoint(point);
			}
		}




		public IList<SimulationManager> ScheduleZeusJobs(int count, string plgridLogin, string plgridPassword)
		{
			throw new NotImplementedException();
		}
		public IList<SimulationManager> ScheduleZeusJobs(int count, IDictionary<string, object> parameters = null)
		{
			throw new NotImplementedException();
		}
		public IList<SimulationManager> SchedulePrivateMachineJobs(int count, PrivateMachineCredentials credentials)
		{
			throw new NotImplementedException();
		}
		public IList<SimulationManager> SchedulePrivateMachineJobs(int count, string credentialsId)
		{
			throw new NotImplementedException();
		}
		public IList<SimulationManager> SchedulePlGridJobs(string plgridCe, int count, string plgridLogin, string plgridPassword, string keyPassphrase)
		{
			throw new NotImplementedException();
		}
		public IList<SimulationManager> SchedulePlGridJobs(string plgridCe, int count, string plgridProxy)
		{
			throw new NotImplementedException();
		}
		public IList<SimulationManager> SchedulePlGridJobs(string plgridCe, int count)
		{
			throw new NotImplementedException();
		}
		public bool IsDone()
		{
			throw new NotImplementedException();
		}
		public void StartWatching()
		{
			throw new NotImplementedException();
		}
		public void StopWatching()
		{
			throw new NotImplementedException();
		}
		public void FillSimulationParamsMap(IList<ValuesMap> results, IList<string> parametersIds)
		{
			throw new NotImplementedException();
		}
	}
}

