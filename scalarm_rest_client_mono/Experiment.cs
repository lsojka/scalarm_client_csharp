using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

namespace Scalarm
{	
	public class Experiment : ScalarmObject
	{
        // TODO: make full experiment model

        public string ExperimentId {get; private set;}

        public Experiment(string experimentId, Client client) : base(client)
        {
            ExperimentId = experimentId;
        }
	}

}

