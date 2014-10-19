using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace Scalarm
{	
	public class InvalidResponseException : Exception
	{
        public IRestResponse Response {get; protected set;}

        public InvalidResponseException(IRestResponse response)
        {
            Response = response;
        }
	}
}

