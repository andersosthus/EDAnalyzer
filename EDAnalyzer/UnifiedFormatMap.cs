using CsvHelper.Configuration;
using EDAnalyzer.Models;

namespace EDAnalyzer
{
	public sealed class UnifiedFormatMap : CsvClassMap<ItemLine>
	{
		public UnifiedFormatMap()
		{
			Map(m => m.SystemName).Index(0);
			Map(m => m.StationName).Index(1);
			Map(m => m.CommodityName).Index(2);
			Map(m => m.SellPrice).Index(3).TypeConverter<MissingNumberConverter>();
			Map(m => m.BuyPrice).Index(4).TypeConverter<MissingNumberConverter>();
			Map(m => m.Demand).Index(5).TypeConverter<MissingNumberConverter>();
			Map(m => m.UpdatedAt).Index(9);
		}
	}
}