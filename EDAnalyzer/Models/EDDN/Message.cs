using System;
using Newtonsoft.Json;

namespace EDAnalyzer.Models.EDDN
{
	public class Message
	{
		[JsonProperty(PropertyName = "buyPrice")]
		public int BuyPrice { get; set; }

		[JsonProperty(PropertyName = "timestamp")]
		public DateTime Timestamp { get; set; }

		[JsonProperty(PropertyName = "stationStock")]
		public int StationStock { get; set; }

		[JsonProperty(PropertyName = "systemName")]
		public string SystemName { get; set; }

		[JsonProperty(PropertyName = "stationName")]
		public string StationName { get; set; }

		[JsonProperty(PropertyName = "demand")]
		public int Demand { get; set; }

		[JsonProperty(PropertyName = "sellPrice")]
		public int SellPrice { get; set; }

		[JsonProperty(PropertyName = "itemName")]
		public string ItemName { get; set; }
	}
}