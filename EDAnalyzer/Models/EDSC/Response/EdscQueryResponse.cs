using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Response
{
	public class EdscQueryResponse
	{
		[JsonProperty(PropertyName = "D")]
		public QueryResponseMetadata Metadata { get; set; }
	}
}