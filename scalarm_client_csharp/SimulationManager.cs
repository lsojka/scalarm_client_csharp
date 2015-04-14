using System;
using RestSharp.Deserializers;
using Newtonsoft.Json;
using System.Collections.Generic;
using Scalarm.ExperimentInput;
using RestSharp;


namespace Scalarm
{
    public class SimulationManager : ScalarmObject
    {
        
        [DeserializeAs(Name = "_id")]
        public string Id {get; private set;}

        [DeserializeAs(Name = "user_id")]
        public string UserId {get; private set;}

        [DeserializeAs(Name = "executor_id")]
        public string ExecutorId {get; private set;}

        [DeserializeAs(Name = "sm_uuid")]
        public string UUID {get; private set;}

        [DeserializeAs(Name = "time_limit")]
        public int TimeLimitMins {get; private set;}

        // TODO: pseudo-enum
		[DeserializeAs(Name = "infrastructure")]
        public string Infrastructure {get; private set;}

        [DeserializeAs(Name = "sm_initialized_at")]
        public DateTime SimInitializedAt {get; private set;}

        [DeserializeAs(Name = "created_at")]
        public DateTime CreatedAt {get; private set;}

        [DeserializeAs(Name = "state")]
        public string State {get; private set;}

		[DeserializeAs(Name = "onsite_monitoring")]
		public bool IsOnSiteMonitored {get; private set;}

        // TODO: enum?
        [DeserializeAs(Name = "error")]
        public string Error {get; private set;}

		[DeserializeAs(Name = "error_log")]
        public string ErrorDetails {get; private set;}

        [DeserializeAs(Name = "name")]
        public string Name {get; private set;}

        // TODO: check!
        // TODO: enum?
        [DeserializeAs(Name = "resource_status")]
        protected string _resourceStatus {get; private set;}

        // spec: scheduler_type, grant_id, nodes, ppn, sm

		/// <summary>
		/// Gets the resource status, which represents present state of resource.
		/// </summary>
		/// <value>The resource status. Possible values:
		/// <list type="bullet">
		/// 	<listheader>
		/// 		<term>state</term>
		/// 		<description>description</description>
		/// 	</listheader>
		/// 	<item>
		/// 		<term>not_available</term>
		/// 		<description>Resource cannot be initialized, because infrastructure is not working properly</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>available</term>
		/// 		<description>The infrastructure is working properly, resource can be initialized</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>initializing</term>
		/// 		<description>The resource is preparing to work (eg. virtual machine is starting)</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>ready</term>
		/// 		<description>The resource is ready, simulation will be started soon</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>running_sm</term>
		/// 		<description>Simulations are working</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>released</term>
		/// 		<description>The resource is freed</description>
		/// 	</item>
		/// </list>
		/// </value>
        public string ResourceStatus
        {
            get {
				return IsOnSiteMonitored ? _resourceStatus : _queryResourceStatus();
            }
        }

        private string _queryResourceStatus()
        {
            return Client.GetResourceStatus(Infrastructure, Id);
        }

		public SimulationManager()
		{
		}

		private void _simulationManagerCommand(string command)
		{
			var request = new RestRequest("infrastructure/simulation_manager_command", Method.POST);

			request.AddParameter("command", command);
			request.AddParameter("record_id", Id);
			request.AddParameter("infrastructure_name", Infrastructure);

			var result = Client.Execute<SimulationManagerCommandResult>(request);

			HandleSimulationManagerCommandResponse(result, command);
		}

		private void HandleSimulationManagerCommandResponse(IRestResponse<SimulationManagerCommandResult> response, String commandName = null)
		{
			Client.ValidateResponseStatus(response);

			var data = response.Data;

			if (data.status == "ok")
			{
				return;
			} else if (data.status == "error") {
				throw new ScalarmException(string.Format("Command ({0}) on SimulationManager {1} failed: {2}",
				                                         String.IsNullOrEmpty(commandName) ? commandName : "unknown",
				                                         Id, data.error_code));
			} else {
				throw new InvalidResponseException(response);
			}
		}

		public void Stop()
		{
			_simulationManagerCommand("stop");
		}

		public void Destroy()
		{
			_simulationManagerCommand("destroy_record");
		}

		public void Restart()
		{
			_simulationManagerCommand("restart");
		}
    }
}

