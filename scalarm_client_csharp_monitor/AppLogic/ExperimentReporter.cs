using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scalarm;

namespace scalarm_client_csharp_monitor.AppLogic
{
    public class ExperimentReporter
    {
        Client client;
        public List<IntermediateResult> intermediateResults;

        public string experimentId { get; set; }
        Experiment experiment;


        public ExperimentReporter(string _experimentId, Client _client)
        {
            experimentId = _experimentId;
            client = _client;
            experiment = client.GetExperimentById<Experiment>(experimentId);

            
            intermediateResults = new List<IntermediateResult>();
            // getMeGoing!
        }

        public void CollectUpdates()
        {
            
        }
    }

    
}
