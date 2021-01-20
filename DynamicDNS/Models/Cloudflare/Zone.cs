using Newtonsoft.Json;

namespace DynamicDNS.Models.Cloudflare
{
	public class Zone
	{
		public Zone()
		{
			Id = string.Empty;
			Name = string.Empty;
		}

		public Zone(string id, string name)
		{
			Id = id;
			Name = name;
		}

		[JsonProperty("id", Required = Required.Always)]
		public string Id { get; set; }

		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; }
	}
}
