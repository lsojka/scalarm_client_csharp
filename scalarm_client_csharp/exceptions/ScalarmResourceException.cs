using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace Scalarm
{	
	public class ScalarmResourceException<T> : Exception
	{
        public ResourceEnvelope<T> Resource
        {
            get;
            protected set;
        }

		public ScalarmResourceException(ResourceEnvelope<T> resource)
		{
            Resource = resource;
		}
	}


}

