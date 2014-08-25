using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

namespace Scalarm
{	
	public class CreateExperimentException : Exception
	{
        public CreateExperimentException(string message) : base(message)
        {
        }
	}

}

