using System;
using ProtoBuf;

namespace EDAnalyzer.Models
{
	[ProtoContract]
	public class ItemLine
	{
		[ProtoMember(1)]
		public string SystemName { get; set; }

		[ProtoMember(2)]
		public string StationName { get; set; }

		[ProtoMember(3)]
		public string CommodityName { get; set; }

		[ProtoMember(4)]
		public int SellPrice { get; set; }

		[ProtoMember(5)]
		public int BuyPrice { get; set; }

		[ProtoMember(6)]
		public int Demand { get; set; }

		[ProtoMember(7)]
		public DateTime UpdatedAt { get; set; }

		[ProtoMember(8)]
		public string Hash { get; set; }

		public object this[string indexer]
		{
			get {
				switch (indexer)
				{
					case "systemname":
						return SystemName;
					case "stationname":
						return StationName;
					case "commodityname":
						return CommodityName;
					case "buyprice":
						return BuyPrice;
					case "sellprice":
						return SellPrice;
					case "demand":
						return Demand;
					case "updatedat":
						return UpdatedAt;
					default:
						return null;
				}
			}
		}
	}
}