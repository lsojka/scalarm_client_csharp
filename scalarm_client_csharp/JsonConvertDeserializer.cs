using System;
using RestSharp.Deserializers;
using Newtonsoft.Json;
using RestSharp;


namespace Scalarm
{
    // Not used because of bug in JSON.net in Mono
    public class JsonConvertDeserializer : IDeserializer
    {
        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public string RootElement {get; set;}

        public string Namespace {get; set;}

        public string DateFormat {get; set;}
    }
}

