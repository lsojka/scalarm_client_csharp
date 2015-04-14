using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

namespace Scalarm
{	
	public class CreateScenarioException : ScalarmException
	{
		public CreateScenarioException(string message) : base(message)
        {
        }
	}

}

