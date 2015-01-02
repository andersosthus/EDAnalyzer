using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using EDAnalyzer.Interfaces;
using EDAnalyzer.Models;
using ReactiveUI;
using Splat;

namespace EDAnalyzer.Commands
{
	public static class AllInterSystemsCommandFactory
	{
		public static ReactiveCommand<Unit> Create(ICollection<Trade> foundTradesList,
			IReadOnlyReactiveList<ItemLine> allItems)
		{
			return ReactiveCommand.CreateAsyncTask(async _ =>
			{
				var lootService = Locator.CurrentMutable.GetService<ICalculateLoot>();
				foundTradesList.Clear();

				var items = allItems.ToList();
				var trades = await lootService.CalculateProfitAcrossSystems(items);

				trades.ForEach(foundTradesList.Add);
			});
		}
	}
}