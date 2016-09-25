using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scalarm;
using System.Threading;

namespace scalarm_client_csharp_monitor.AppLogic
{
    public class ExperimentChecker
    {
        /*
        Client client;
        public List<IntermediateResult> intermediateResults;
        
        public string experimentId { get; set; }
        Experiment experiment;
        

        public ExperimentChecker(string _experimentId, Client _client)
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
         * */
        private int invokeCount;
        private int maxCount;

        public ExperimentChecker(int _count)
        {
            invokeCount = 0;
            maxCount = _count;
        }

        // method called by timer delegate
        public void CheckStatus(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            invokeCount++;

            if (invokeCount == maxCount)
            {
                invokeCount = 0;
                autoEvent.Set();
            }
        }
    }

    
}
