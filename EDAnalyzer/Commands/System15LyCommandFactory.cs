using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using EDAnalyzer.Interfaces;
using EDAnalyzer.Models;
using EDAnalyzer.Models.EDSC.Response;
using EDAnalyzer.Services;
using ReactiveUI;
using Splat;

namespace EDAnalyzer.Commands
{
	public static class System15LyCommandFactory
	{
		public static ReactiveCommand<Unit> Create(ICollection<Trade> foundTradesList,
			IReadOnlyReactiveList<ItemLine> allItems)
		{
			return ReactiveCommand.CreateAsyncTask(async _ =>
			{
				var edsc = Locator.CurrentMutable.GetService<IQueryEdsc>();
				var lootService = Locator.CurrentMutable.GetService<ICalculateLoot>();
				foundTradesList.Clear();
				var systems = await edsc.QueryForSystemNameAsync(_.ToString().ToLower());

				FoundSystem system;
				if (systems.Metadata.FoundSystems.Count() != 1)
				{
					Debug.WriteLine("Found several systems - Trying to guess...");
					var matches =
						systems.Metadata.FoundSystems.Where(x => x.Name.Trim().ToLower().Equals(_.ToString().ToLower())).ToList();
					if (!matches.Any())
					{
						Debug.WriteLine("No matches found");
						return;
					}

					system = matches.First();
					Debug.WriteLine(string.Format("Guessed on {0}", system.Name));
				}
				else
				{
					system = systems.Metadata.FoundSystems.First();
					Debug.WriteLine(string.Format("Found {0}", system.Name));
				}

				var systemsInRange = await edsc.QueryForSystemsWithinRangeAsync(15.00f, system.Coordinates);

				var systemNames = systemsInRange.Metadata.Distances.Select(x => x.SystemName.Trim().ToLower()).ToList();
				systemNames.Add(_.ToString().ToLower());

				var items = allItems.ToList().ToList();
				var trades = await lootService.CalculateProfitAcrossSeveralSystemsAsync(items, systemNames);

				trades.ForEach(foundTradesList.Add);
			});
		}
	}
}