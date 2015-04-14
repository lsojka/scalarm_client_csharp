using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;

namespace Scalarm
{
	public class CredentialsValidationException : ScalarmException
	{
		public string RecordId { get; private set; }

		public CredentialsValidationException(string recordId) : base("Credentials added as " + recordId + ", but cannot be validated")
		{
			RecordId = recordId;
		}
	}
}

