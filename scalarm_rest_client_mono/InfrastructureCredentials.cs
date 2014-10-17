using System;
using RestSharp.Deserializers;

namespace Scalarm
{
	public class InfrastructureCredentials : ScalarmObject
	{
		#region model

		[DeserializeAs(Name = "_id")]
		public string Id {get; private set;}

		[DeserializeAs(Name = "user_id")]
		public string UserId {get; private set;}

		[DeserializeAs(Name = "invalid")]
		public bool IsInvalid {get; private set;}

		#endregion

		public InfrastructureCredentials()
		{
		}
	}
}

