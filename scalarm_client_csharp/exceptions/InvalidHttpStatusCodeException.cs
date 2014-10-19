using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace Scalarm
{	
	public class InvalidHttpStatusCodeException : InvalidResponseException
	{
        public InvalidHttpStatusCodeException(IRestResponse response) : base(response)
        {
        }
	}


}

