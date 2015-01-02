using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDDN
{
	public class Root
	{
		[JsonProperty(PropertyName = "header")]
		public Header Header { get; set; }

		[JsonProperty(PropertyName = "schemaRef")]
		public string SchemaRef { get; set; }

		[JsonProperty(PropertyName = "message")]
		public Message Message { get; set; }
	}
}