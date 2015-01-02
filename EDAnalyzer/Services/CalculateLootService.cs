using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

		public async Task<List<Trade>> CalculateProfitInOneSystem(IList<ItemLine> itemsInSystem)
		{
			var trades = await FindProfitableTradesInterSystemAsync(itemsInSystem);

			if (!trades.Any())
				Debug.WriteLine("No profit trades found!");

			var profitableTrades = trades.OrderByDescending(x => x.Profit).Take(5).ToList();
			return profitableTrades;
		}

		public async Task<List<Trade>> CalculateProfitAcrossSystems(IList<ItemLine> items)
		{
			var systemList = items.DistinctBy(x => x.SystemName.ToLower());
			var trades = new List<Trade>();

			foreach (
				var currentSystem in
					systemList.Select(system => items.Where(x => x.SystemName.ToLower().Equals(system.SystemName.ToLower())).ToList()))
			{
				trades.AddRange(await FindProfitableTradesInterSystemAsync(currentSystem));
			}

			var profitableTrades = trades.OrderByDescending(x => x.Profit).Take(10).ToList();
			return profitableTrades;
		}

		public async Task<List<Trade>> CalculateProfitAcrossSeveralSystemsAsync(IList<ItemLine> items,
			IList<string> systemNames)
		{
			Debug.WriteLine("Started processing...");
			var systemList =
				items.DistinctBy(x => x.SystemName.ToLower()).Where(x => systemNames.Contains(x.SystemName.ToLower())).ToList();
			Debug.WriteLine("{0} systems to be checked...", systemList.Count);

			var itemsToCheck = new List<ItemLine>();

			foreach (
				var currentSystem in
					systemList.Select(
						currentSystem => items.Where(i => i.SystemName.ToLower().Equals(currentSystem.SystemName.ToLower()))))
			{
				itemsToCheck.AddRange(currentSystem);
			}

			var trades = await FindProfitableTradesInterSystemAsync(itemsToCheck);

			var profitableTrades = trades.OrderByDescending(x => x.Profit).Take(20).ToList();
			Debug.WriteLine("Ended processing...");
			return profitableTrades;
		}

		private static async Task<List<Trade>> FindProfitableTradesInterSystemAsync(IList<ItemLine> itemsToCheck)
		{
			return await Task.Factory.StartNew(() =>
			{
				var trades = new List<Trade>();

				var stations = itemsToCheck.DistinctBy(x => x.StationName.ToLower()).ToList();
				if (stations.Count() <= 1)
					return trades;

				var byStation = itemsToCheck.GroupBy(x => x.StationName.ToLower());
				foreach (var station in byStation)
				{
					Debug.WriteLine(string.Format("Checking station {0}", station.Key));
					foreach (var itemLine in station)
					{
						var grouping = station;
						var item = itemLine;
						if (item.BuyPrice == 0)
							continue;

						var itemInOtherStations = itemsToCheck
							.Where(x => !x.StationName.ToLower().Equals(grouping.Key.ToLower()))
							.Where(x => x.CommodityName.Equals(item.CommodityName))
							.ToList();

						if (!itemInOtherStations.Any())
							continue;

						foreach (var sellStation in itemInOtherStations)
						{
							if (sellStation.Demand == 0 || sellStation.Demand <= 0)
								continue;

							var foundTrade = new Trade
							{
								BuyPrice = item.BuyPrice,
								Commodity = item.CommodityName,
								BuyStation = item.StationName,
								FromSystem = item.SystemName,
								ToSystem = sellStation.SystemName,
								SellStation = sellStation.StationName,
								SellPrice = sellStation.SellPrice,
								Demand = sellStation.Demand,
								TradeType = (item.SystemName.Equals(sellStation.SystemName)) ? Constants.TradeType.IntraSystem : Constants.TradeType.CrossSystem
							};
							foundTrade.Profit = foundTrade.SellPrice - foundTrade.BuyPrice;

							trades.Add(foundTrade);
						}
					}
				}

				return trades;
			});
		}
	}
}