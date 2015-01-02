using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Response
{
	public class QueryInput
	{
		[JsonProperty(PropertyName = "status")]
		public InputStatus Status { get; set; }
	}
}