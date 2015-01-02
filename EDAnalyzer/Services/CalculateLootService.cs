using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EDAnalyzer.Interfaces;
using EDAnalyzer.Models;
using MoreLinq;

namespace EDAnalyzer.Services
{
	public class CalculateLootService : ICalculateLoot
	{
		public CalculateLootService()
		{
		}

		public List<Trade> CalculateProfitInOneSystem(IList<ItemLine> itemsInSystem)
		{
			var trades = FindProfitableTradesInterSystem(itemsInSystem);

			if(!trades.Any())
				Debug.WriteLine("No profit trades found!");

			var profitableTrades = trades.OrderByDescending(x => x.Profit).Take(5).ToList();
			//profitableTrades.ForEach(trade => Debug.WriteLine("Found trade. Buy {0} in {1} for {2}. Sell in {3} for {4}. Profit: {5} - Demand {6}", trade.Commodity, trade.BuyStation, trade.BuyPrice, trade.SellStation, trade.SellPrice, trade.Profit, trade.Demand));
			return profitableTrades;
		}

		public void CalculateProfitAcrossSystems(IList<ItemLine> items)
		{
			var systemList = items.DistinctBy(x => x.SystemName.ToLower());
			var trades = new List<Trade>();

			foreach (var currentSystem in systemList.Select(system => items.Where(x => x.SystemName.ToLower().Equals(system.SystemName.ToLower())).ToList()))
			{
				trades.AddRange(FindProfitableTradesInterSystem(currentSystem));
			}

			var profitableTrades = trades.OrderByDescending(x => x.Profit).Take(10).ToList();
			profitableTrades.ForEach(trade => Debug.WriteLine("Found trade. Buy {0} in {1} for {2}. Sell in {3} for {4}. Profit: {5} - Demand {6} ({7})", trade.Commodity, trade.BuyStation, trade.BuyPrice, trade.SellStation, trade.SellPrice, trade.Profit, trade.Demand, trade.SystemName));
		}

		private static List<Trade> FindProfitableTradesInterSystem(IList<ItemLine> itemsInSystem)
		{
			var trades = new List<Trade>();

			var stations = itemsInSystem.DistinctBy(x => x.StationName.ToLower()).ToList();
			if (stations.Count() <= 1)
				return trades;

			var byStation = itemsInSystem.GroupBy(x => x.StationName.ToLower());
			foreach (var station in byStation)
			{
				foreach (var itemLine in station)
				{
					var grouping = station;
					var item = itemLine;
					if (item.BuyPrice == 0)
						continue;

					var itemInOtherStation = itemsInSystem
						.Where(x => !x.StationName.ToLower().Equals(grouping.Key.ToLower()))
						.Where(x => x.CommodityName.Equals(item.CommodityName))
						.ToList();

					if (!itemInOtherStation.Any())
						continue;

					foreach (var sellStation in itemInOtherStation)
					{
						if (sellStation.Demand == 0 || sellStation.Demand <= 0)
							continue;

						var foundTrade = new Trade
						{
							BuyPrice = item.BuyPrice,
							Commodity = item.CommodityName,
							BuyStation = item.StationName,
							SystemName = item.SystemName,
							SellStation = sellStation.StationName,
							SellPrice = sellStation.SellPrice,
							Demand = sellStation.Demand,
							TradeType = Constants.TradeType.IntraSystem
						};
						foundTrade.Profit = foundTrade.SellPrice - foundTrade.BuyPrice;

						trades.Add(foundTrade);
					}
				}
			}

			return trades;
		}
	}
}