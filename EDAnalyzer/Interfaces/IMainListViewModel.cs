using System.Reactive;
using EDAnalyzer.Models;
using ReactiveUI;

namespace EDAnalyzer.Interfaces
{
	public interface IMainListViewModel
	{
		ReactiveCommand<ItemLine> LoadItemsFromDisk { get; }
		int TotalItemsCount { get; }
		bool Saving { get; }
		IReadOnlyReactiveList<ItemLine> AllItems { get; }
		ReactiveList<ItemLine> DeletionList { get; set; }
		ReactiveCommand<Unit> SaveAsync();
	}
}