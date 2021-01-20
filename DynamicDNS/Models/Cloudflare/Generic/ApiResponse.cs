using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DynamicDNS.Models.Cloudflare.Generic
{
	public class ApiResponse : ApiResponse<object> {}

	public class ApiResponse<T> where T : class
    {

		public ApiResponse()
		{
			Success = false;
			Errors = new Collection<ApiError>();
			Messages = new Collection<string>();
		}

		public ApiResponse(
			bool success,
			IEnumerable<ApiError> errors, 
			IEnumerable<string> messages,
			T result)
		{
			Success = success;
			Errors = errors;
			Messages = messages;
			Result = result;
		}

		[JsonProperty("success", Required = Required.Always)]
		public bool Success { get; set; }

		[JsonProperty("errors", NullValueHandling = NullValueHandling.Include)]
		public IEnumerable<ApiError> Errors { get; set; }

		[JsonProperty("messages", NullValueHandling = NullValueHandling.Include)]
		public IEnumerable<string> Messages { get; set; }

		[JsonProperty("result", NullValueHandling = NullValueHandling.Include)]
		public T Result { get; set; }
	}
}
