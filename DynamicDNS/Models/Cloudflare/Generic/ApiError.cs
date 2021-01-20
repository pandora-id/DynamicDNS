using Newtonsoft.Json;

namespace DynamicDNS.Models.Cloudflare.Generic
{
	public class ApiError
	{
		public ApiError()
		{
			Code = 0;
			Message = string.Empty;
		}

		public ApiError(uint code, string message)
		{
			Code = code;
			Message = message;
		}

		[JsonProperty("code", Required = Required.Always)]
		public uint Code { get; set; }

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; set; }
	}
}
