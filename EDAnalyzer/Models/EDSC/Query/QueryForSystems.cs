using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Query
{
	public class QueryForSystems
	{
		[JsonProperty(PropertyName = "data")]
		public QueryParams Query { get; set; }
	}
}