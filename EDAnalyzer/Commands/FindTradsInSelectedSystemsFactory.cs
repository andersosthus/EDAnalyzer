using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using EDAnalyzer.Interfaces;
using EDAnalyzer.Models;
using ReactiveUI;
using Splat;

namespace EDAnalyzer.Commands
{
	public static class FindTradsInSelectedSystemsFactory
	{
		public static ReactiveCommand<Unit> Create(ICollection<Trade> foundTradesList,
			IReadOnlyReactiveList<ItemLine> allItems, IList<string> selectedSystems)
		{
			return ReactiveCommand.CreateAsyncTask(async _ =>
			{
				var lootService = Locator.CurrentMutable.GetService<ICalculateLoot>();
				foundTradesList.Clear();

				var systems = selectedSystems.Select(x => x.ToLower()).ToList();
				var trades = await lootService.CalculateProfitAcrossSeveralSystemsAsync(allItems, systems);

				trades.ForEach(foundTradesList.Add);
			});
		}
	}
}