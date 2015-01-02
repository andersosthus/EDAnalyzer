using System.ComponentModel;
using System.Reactive;
using EDAnalyzer.Models;
using ReactiveUI;

namespace EDAnalyzer.Interfaces
{
	public interface IMainViewModel : IRoutableViewModel
	{
		IReactiveDerivedList<ItemLine> Items { get; set; }
		string OrderField { get; set; }
		ListSortDirection OrderDirection { get; set; }
		Trade SelectedTrade { get; set; }
		ReactiveCommand<Unit> SaveAsyncCommand { get; }
		ReactiveCommand<object> PurgeDataCommand { get; }
		ReactiveCommand<object> FilterCommand { get; }
		ReactiveCommand<Unit> InterSystemCommand { get; }
		ReactiveCommand<Unit> AllInterSystemsCommand { get; }
		ReactiveCommand<Unit> System15LyCommand { get; }
	}
}