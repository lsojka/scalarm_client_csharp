using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

namespace Scalarm
{	
	public class Experiment
	{
        public Client Client {get; private set;}
        public string ExperimentId {get; private set;}

        public Experiment(string experimentId, Client client)
        {
            Client = client;
            ExperimentId = experimentId;
        }
	}

}

