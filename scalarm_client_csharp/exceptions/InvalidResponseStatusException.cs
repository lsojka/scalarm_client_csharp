using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace Scalarm
{	
	public class InvalidResponseStatusException : InvalidResponseException
	{
        public InvalidResponseStatusException(IRestResponse response) : base(response)
        {
        }
	}
}

