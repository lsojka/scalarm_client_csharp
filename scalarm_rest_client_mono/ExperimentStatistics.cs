using System;
using RestSharp.Deserializers;
using Newtonsoft.Json;


namespace Scalarm
{
    public class ExperimentStatistics
    {
        [JsonProperty(PropertyName = "all")]
        public int All {get; private set;}

        [JsonProperty(PropertyName = "sent")]
        public int InProgress {get; private set;}

        [JsonProperty(PropertyName = "done_num")]
        public int Done {get; private set;}

        public ExperimentStatistics()
        {
        }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
    }
}

