using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDSC.Query
{
	public class QueryFilter
	{
		[JsonProperty(PropertyName = "date")]
		public string NoResultsOlderThan = "2014-09-18 12:34:56";

		[JsonIgnore]
		private int _confidenceRating = 5;

		[JsonProperty(PropertyName = "knownstatus")]
		public EdscConstants.KnownStatus KnownStatus { get; set; }

		[JsonProperty(PropertyName = "systemname")]
		public string SystemNameToQuery { get; set; }

		[JsonProperty(PropertyName = "cr")]
		public int ConfidenceRating
		{
			get { return _confidenceRating; }
			set { _confidenceRating = value; }
		}

		[JsonProperty(PropertyName = "coordsphere")]
		public CoordSphere CoordSphere { get; set; }
	}
}