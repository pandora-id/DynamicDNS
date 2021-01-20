using Newtonsoft.Json;

namespace DynamicDNS.Models.Cloudflare
{
	public class DnsRecord
	{
		public DnsRecord()
		{
			Id = string.Empty;
			ZoneId = string.Empty;
			Type = string.Empty;
			Name = string.Empty;
			Content = string.Empty;
		}

		public DnsRecord(string id, string zoneId, string type, string name, string content, uint ttl, bool proxied)
		{
			Id = id;
			ZoneId = zoneId;
			Type = type;
			Name = name;
			Content = content;
			Ttl = ttl;
			Proxied = proxied;
		}

		[JsonProperty("id", Required = Required.Always)]
		public string Id { get; set; }

		[JsonProperty("zone_id", Required = Required.Always)]
		public string ZoneId { get; set; }

		[JsonProperty("type", Required = Required.Always)]
		public string Type { get; set; }

		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; }

		[JsonProperty("content", Required = Required.Always)]
		public string Content { get; set; }

		[JsonProperty("ttl", Required = Required.Always)]
		public uint Ttl { get; set; }

		[JsonProperty("proxied", Required = Required.Always)]
		public bool Proxied { get; set; }
	}

}
