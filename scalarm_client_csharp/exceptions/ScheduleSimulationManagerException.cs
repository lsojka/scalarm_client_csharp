using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

namespace Scalarm
{	
	public class ScheduleSimulationManagerException : ScalarmException
	{
        public string ErrorCode { get; private set; }

        public ScheduleSimulationManagerException(string errorCode, string message)
            :
                base(String.Format("{0}: {1}", errorCode, message))
        {
            ErrorCode = errorCode;
        }
	}

}

