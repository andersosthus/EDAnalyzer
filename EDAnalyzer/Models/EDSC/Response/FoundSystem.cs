using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Response
{
	public class FoundSystem
	{
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "coord")]
		public float[] Coordinates { get; set; }

		[JsonProperty(PropertyName = "")]
		public int Confidence { get; set; }

		[JsonProperty(PropertyName = "commandercreate")]
		public string CreatedBy { get; set; }

		[JsonProperty(PropertyName = "createdate")]
		public string CreatedAt { get; set; }

		[JsonProperty(PropertyName = "commanderupdate")]
		public string UpdatedBy { get; set; }

		[JsonProperty(PropertyName = "updatedate")]
		public string UpdatedAt { get; set; }
	}
}