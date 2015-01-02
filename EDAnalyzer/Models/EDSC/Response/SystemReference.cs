using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Response
{
	public class SystemReference
	{
		[JsonProperty(PropertyName = "name")]
		public string SystemName { get; set; }

		[JsonProperty(PropertyName = "dist")]
		public float Distance { get; set; }
	}
}