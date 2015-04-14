using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace Scalarm
{	
	public class ScalarmResourceException<T> : ScalarmException
	{
        public ResourceEnvelope<T> Resource
        {
            get;
            protected set;
        }

		public ScalarmResourceException(ResourceEnvelope<T> resource) : base(typeof(T).Name)
		{
            Resource = resource;
		}
	}


}

