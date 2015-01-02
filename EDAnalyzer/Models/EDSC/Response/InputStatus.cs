using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Response
{
	public class InputStatus
	{
		[JsonProperty(PropertyName = "statusnum")]
		public int Code { get; set; }

		[JsonProperty(PropertyName = "msg")]
		public string Message { get; set; }
	}
}