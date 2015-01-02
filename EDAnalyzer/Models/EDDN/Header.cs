using System;
using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDDN
{
	public class Header
	{
		[JsonProperty(PropertyName = "softwareVersion")]
		public string SoftwareVersion { get; set; }

		[JsonProperty(PropertyName = "gatewayTimestamp")]
		public DateTime GatewayTimestamp { get; set; }

		[JsonProperty(PropertyName = "softwareName")]
		public string SoftwareName { get; set; }

		[JsonProperty(PropertyName = "uploaderID")]
		public string UploaderId { get; set; }
	}
}