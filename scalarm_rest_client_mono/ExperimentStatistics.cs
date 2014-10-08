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

		// strange bug - JsonProperty does not work with done_num (underscore problem?)
		[DeserializeAs(Name = "done_num")]
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

