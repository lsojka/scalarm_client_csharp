using System;
using RestSharp.Deserializers;

namespace Scalarm
{
	public class PrivateMachineCredentials : InfrastructureCredentials
	{
		[DeserializeAs(Name = "host")]
		public string Host {get; private set;}

		[DeserializeAs(Name = "port")]
		public int Port {get; private set;}

		[DeserializeAs(Name = "login")]
		public string Login {get; private set;}

		public PrivateMachineCredentials()
		{
		}

		// TODO
		public void ChangePassword(string newPassword)
		{
			throw new NotImplementedException();
		}
	}
}

