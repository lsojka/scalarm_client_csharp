using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

namespace Scalarm
{	
	public class ScheduleSimulationManagerException : Exception
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

