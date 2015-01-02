namespace EDAnalyzer.Models
{
	public class Trade
	{
		public string Commodity { get; set; }
		public string SystemName { get; set; }
		public string BuyStation { get; set; }
		public string SellStation { get; set; }
		public int BuyPrice { get; set; }
		public int SellPrice { get; set; }
		public int Profit { get; set; }
		public int Demand { get; set; }
		public Constants.TradeType TradeType { get; set; }
	}
}