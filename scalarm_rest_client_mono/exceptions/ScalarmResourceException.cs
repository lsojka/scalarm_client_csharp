using System;
using System.Net;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using RestSharp;

namespace Scalarm
{	
	public class ScalarmResourceException : Exception
	{
        public ScalarmResource<SimulationScenario> Resource
        {
            get;
            protected set;
        }

		public ScalarmResourceException(ScalarmResource<SimulationScenario> resource)
		{
            Resource = resource;
		}
	}


}

