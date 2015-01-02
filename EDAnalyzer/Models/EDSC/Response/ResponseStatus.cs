using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Response
{
	public class ResponseStatus
	{
		[JsonProperty(PropertyName = "input")]
		public QueryInput[] QueryInput { get; set; }
	}
}