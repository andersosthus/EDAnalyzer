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
		ReactiveCommand<Unit> SaveAsyncCommand { get; }
		ReactiveCommand<object> PurgeDataCommand { get; }
		ReactiveCommand<object> FilterCommand { get; }
		ReactiveCommand<object> InterSystemCommand { get; }
		ReactiveCommand<object> AllInterSystemsCommand { get; }
	}
}