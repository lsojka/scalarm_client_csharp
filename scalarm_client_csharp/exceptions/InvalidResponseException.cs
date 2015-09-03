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

		public override string ToString()
		{
			return string.Format("InvalidResponseException: " +
				"StatusCode: {0}, " +
				"Body: {1}",
				Response.StatusCode,
				Response.Content
			);
		}
	}
}

