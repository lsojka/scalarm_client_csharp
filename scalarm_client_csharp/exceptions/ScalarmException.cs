using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

namespace Scalarm
{	
	public class ScalarmException : Exception
	{
		public ScalarmException(string message) : base(message)
        {
        }
	}

}

