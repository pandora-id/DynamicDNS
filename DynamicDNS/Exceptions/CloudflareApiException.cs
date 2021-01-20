using System;

namespace DynamicDNS.Exceptions
{
	public class CloudflareApiException : Exception
	{
		public CloudflareApiException(uint code, string message) : base(message)
		{
			Code = code;
		}

		public uint Code { get; set; }
	}
}
