using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

namespace Scalarm
{	
	public class CreateExperimentException : ScalarmException
	{
        public CreateExperimentException(string message) : base(message)
        {
        }
	}

}

