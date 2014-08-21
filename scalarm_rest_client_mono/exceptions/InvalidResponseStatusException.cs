using System;
using System.Net;
using System.Collections.Generic;
using System.Web.Script.Serialization;
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

