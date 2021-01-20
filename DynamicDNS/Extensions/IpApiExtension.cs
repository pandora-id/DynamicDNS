using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DynamicDNS.Extensions
{
    public static class IpSbApiExtension
    {
        public static async Task<string> GetPublicIpv4Address(this HttpClient httpClient)
        {
			var requestUri = new Uri("https://api-ipv4.ip.sb/ip");

			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			var response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (response is null)
			{
				throw new NullReferenceException("HTTP Response Content is null.");
			}

			return response.Split('\n').First();
		}

		public static async Task<string> GetPublicIpv6Address(this HttpClient httpClient)
		{
			var requestUri = new Uri("https://api-ipv6.ip.sb/ip");

			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			var response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (response is null)
			{
				throw new NullReferenceException("HTTP Response Content is null.");
			}

			return response.Split('\n').First();
		}
	}
}
