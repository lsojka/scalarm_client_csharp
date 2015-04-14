using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;

namespace Scalarm
{	
	public class RegisterSimulationScenarioException : ScalarmException
	{
		public RegisterSimulationScenarioException() : base("Simulation scenario registration failed")
		{
		}
	}

}

