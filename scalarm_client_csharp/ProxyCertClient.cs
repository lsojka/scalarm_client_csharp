using System;
using RestSharp;
using System.IO;

namespace Scalarm
{
	public class ProxyCertClient : Client
	{
		public ProxyCertClient (string baseUrl, string proxyCertificate) : base(baseUrl)
		{
			_addProxyHeader (proxyCertificate);
		}

		public ProxyCertClient (string baseUrl, FileStream proxyCertificate) : base(baseUrl)
		{
			using (StreamReader reader = new StreamReader(proxyCertificate)) {
				_addProxyHeader (reader.ReadToEnd());
			}
		}

		private void _addProxyHeader(string proxyContent) {
			this.AddDefaultHeader("X509_PROXY_CERT", PrepareStringForHeader(proxyContent));
		}
	}
}

