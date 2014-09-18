using System;
using RestSharp.Deserializers;
using Newtonsoft.Json;
using System.Collections.Generic;
using Scalarm.ExperimentInput;


namespace Scalarm
{
    public class SimulationManager : ScalarmObject
    {
        
        [DeserializeAs(Name = "_id")]
        public string Id {get; private set;}

        [JsonProperty(PropertyName = "user_id")]
        public string UserId {get; private set;}

        [JsonProperty(PropertyName = "executor_id")]
        public string ExecutorId {get; private set;}

        [JsonProperty(PropertyName = "sm_uuid")]
        public string UUID {get; private set;}

        [JsonProperty(PropertyName = "time_limit")]
        public int TimeLimitMins {get; private set;}

        // TODO: pseudo-enum
        [JsonProperty(PropertyName = "infrastructure")]
        public string InfrastructureName {get; private set;}

        [JsonProperty(PropertyName = "sm_initialized_at")]
        public DateTime SimInitializedAt {get; private set;}

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt {get; private set;}

        [JsonProperty(PropertyName = "state")]
        public string State {get; private set;}

        [JsonProperty(PropertyName = "infrastructure_side_monitoring")]
        public bool IsInfrastructureSideMonitored {get; private set;}

        // TODO: enum?
        [JsonProperty(PropertyName = "error")]
        public string Error {get; private set;}

        [JsonProperty(PropertyName = "error_log")]
        public string ErrorDetails {get; private set;}

        [JsonProperty(PropertyName = "name")]
        public string Name {get; private set;}

        // TODO: check!
        // TODO: enum?
        [JsonProperty(PropertyName = "resource_status")]
        protected string _resourceStatus {get; private set;}

        // spec: scheduler_type, grant_id, nodes, ppn, sm

        public string ResourceStatus
        {
            get {
                return IsInfrastructureSideMonitored ? _resourceStatus : _queryResourceStatus();
            }
        }

        private string _queryResourceStatus()
        {
            return Client.GetResourceStatus(InfrastructureName, Id);
        }

        public SimulationManager()
        {
        }
    }
}

