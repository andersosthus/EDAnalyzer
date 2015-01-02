using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Query
{
	public class CoordSphere
	{
		[JsonProperty(PropertyName = "radius")]
		public float Radius { get; set; }

		[JsonProperty(PropertyName = "origin")]
		public float[] Origin { get; set; }
	}
}