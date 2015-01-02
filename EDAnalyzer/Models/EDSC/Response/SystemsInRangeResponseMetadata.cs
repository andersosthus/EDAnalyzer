using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Response
{
	public class SystemsInRangeResponseMetadata
	{
		[JsonProperty(PropertyName = "ver")]
		public float Version { get; set; }

		[JsonProperty(PropertyName = "date")]
		public string Date { get; set; }

		[JsonProperty(PropertyName = "status")]
		public ResponseStatus ResponseStatus { get; set; }

		[JsonProperty(PropertyName = "distances")]
		public Distance[] Distances { get; set; }
	}
}