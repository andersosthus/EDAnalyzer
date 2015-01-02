using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using EDAnalyzer.Interfaces;
using EDAnalyzer.Models;
using ReactiveUI;
using Splat;

namespace EDAnalyzer.Commands
{
	public static class InterSystemCommandFactory
	{
		public static ReactiveCommand<Unit> Create(ICollection<Trade> foundTradesList,
			IReadOnlyReactiveList<ItemLine> allItems)
		{
			return ReactiveCommand.CreateAsyncTask(async _ =>
			{
				var lootService = Locator.CurrentMutable.GetService<ICalculateLoot>();
				foundTradesList.Clear();

				var itemsInSystem = allItems.Where(x => x.SystemName.ToLower().Equals(_.ToString().ToLower())).ToList();
				var trades = await lootService.CalculateProfitInOneSystem(itemsInSystem);

				trades.ForEach(foundTradesList.Add);
			});
		}
	}
}