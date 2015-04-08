using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;
using System.Threading;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using RestSharp.Deserializers;

namespace Scalarm
{
	public class NoActiveSimulationManagersException : Exception
	{
		public NoActiveSimulationManagersException(string message="") : base(message)
		{
		}
	}
}

