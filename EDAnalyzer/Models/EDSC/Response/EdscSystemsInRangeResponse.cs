using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Response
{
	public class EdscSystemsInRangeResponse
	{
		[JsonProperty(PropertyName = "D")]
		public SystemsInRangeResponseMetadata Metadata { get; set; }
		
	}
}