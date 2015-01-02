using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Response
{
	public class Distance
	{
		[JsonProperty(PropertyName = "name")]
		public string SystemName { get; set; }

		[JsonProperty(PropertyName = "refs")]
		public SystemReference[] References { get; set; }
	}
}