using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Query
{
	public class QueryParams
	{
		[JsonProperty(PropertyName = "ver")]
		public EdscConstants.ApiVersion ApiVersion = EdscConstants.ApiVersion.Ver2;

		[JsonProperty(PropertyName = "outputmode")]
		public EdscConstants.OutputMode OutputMode = EdscConstants.OutputMode.Verbose;

		[JsonProperty(PropertyName = "test")]
		public bool UseTestDb { get; set; }

		[JsonProperty(PropertyName = "filter")]
		public QueryFilter QueryFilter { get; set; }
	}
}