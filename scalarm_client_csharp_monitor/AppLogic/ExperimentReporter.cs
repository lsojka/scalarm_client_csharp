using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scalarm;

namespace scalarm_client_csharp_monitor.AppLogic
{
    class ExperimentReporter
    {
        public string experimentId { get; set; }

        public ExperimentReporter(string _experimentId)
        {
            experimentId = _experimentId;
            // getMeGoing!
        }
    }

    
}
