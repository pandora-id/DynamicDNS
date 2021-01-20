using DynamicDNS.Common;
using DynamicDNS.Exceptions;
using DynamicDNS.Models.Cloudflare.Generic;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DynamicDNS.Extensions
{
	public static class CloudflareApiExtension
	{
		private const string CloudflareApiBaseUrl = "https://api.cloudflare.com/client/v4";

		private static StringContent CreateJsonStringContent<T>(T content)
		{
			var stringContent = JsonConvert.SerializeObject(content);

			return new StringContent(stringContent);
		}

		private static void DisposeObjects(
			HttpClient httpClient, 
			HttpRequestMessage httpRequestMessage,
			HttpResponseMessage httpResponseMessage)
        {
			httpRequestMessage.Dispose();
			httpResponseMessage.Dispose();
			httpClient.Dispose();
		}

		private static HttpClient ProcessHttpClient(this HttpClient httpClient)
        {
			var cloudflareApiToken = Configuration.GetCloudflareApiToken();
			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cloudflareApiToken);

			return httpClient;
		}

		private static T ProcessResponse<T>(this ApiResponse<T> response) where T : class
		{
			if (response.Success)
			{
				return response.Result as T;
			}

			var error = response.Errors.FirstOrDefault();
			if (error != null)
			{
				throw new CloudflareApiException(error.Code, error.Message);
			}

			throw new CloudflareApiException(0, "CloudFlare API operation unsuccessful.");
		}

		private static async Task<T> ReadFromJsonAsync<T>(this HttpContent httpContent) where T : class
        {
			var contentString = await httpContent.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<T>(contentString);
		}

		public static async Task<T> GetCloudflare<T>(this HttpClient httpClient, string endpoint) where T : class
		{
			httpClient.ProcessHttpClient();
			var requestUri = new Uri($"{CloudflareApiBaseUrl}/{endpoint}");

			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<ApiResponse<T>>();

			if (response is null)
			{
				throw new NullReferenceException("HTTP Response Content is null.");
			}

			DisposeObjects(httpClient, httpRequestMessage, httpResponseMessage);
			return response.ProcessResponse();
		}

		public static async Task<T> PostCloudflare<T>(this HttpClient httpClient, string endpoint, object content) where T : class
		{
			httpClient.ProcessHttpClient();
			var requestUri = new Uri($"{CloudflareApiBaseUrl}/{endpoint}");

			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri) {
				Content = CreateJsonStringContent(content)
			};
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<ApiResponse<T>>();
			if (response is null)
			{
				throw new NullReferenceException("HTTP Response Content is null.");
			}

			DisposeObjects(httpClient, httpRequestMessage, httpResponseMessage);
			return response.ProcessResponse();
		}

		public static async Task<T> PutCloudflare<T>(this HttpClient httpClient, string endpoint, object content) where T : class
		{
			httpClient.ProcessHttpClient();
			var requestUri = new Uri($"{CloudflareApiBaseUrl}/{endpoint}");

			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri)
			{
				Content = CreateJsonStringContent(content)
			};
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<ApiResponse<T>>();
			if (response is null)
			{
				throw new NullReferenceException("HTTP Response Content is null.");
			}

			DisposeObjects(httpClient, httpRequestMessage, httpResponseMessage);
			return response.ProcessResponse();
		}

		public static async Task<T> PatchCloudflare<T>(this HttpClient httpClient, string endpoint, object content) where T : class
		{
			httpClient.ProcessHttpClient();
			var requestUri = new Uri($"{CloudflareApiBaseUrl}/{endpoint}");

			var httpRequestMessage = new HttpRequestMessage(ExtendedHttpMethod.Patch, requestUri)
			{
				Content = CreateJsonStringContent(content)
			};
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<ApiResponse<T>>();
			if (response is null)
			{
				throw new NullReferenceException("HTTP Response Content is null.");
			}

			DisposeObjects(httpClient, httpRequestMessage, httpResponseMessage);
			return response.ProcessResponse();
		}

		public static async Task<T> DeleteCloudflare<T>(this HttpClient httpClient, string endpoint) where T : class
		{
			httpClient.ProcessHttpClient();
			var requestUri = new Uri($"{CloudflareApiBaseUrl}/{endpoint}");

			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<ApiResponse<T>>();
			if (response is null)
			{
				throw new NullReferenceException("HTTP Response Content is null.");
			}

			DisposeObjects(httpClient, httpRequestMessage, httpResponseMessage);
			return response.ProcessResponse();
		}
	}
}
