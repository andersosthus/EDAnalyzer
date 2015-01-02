using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using EDAnalyzer.Commands;
using EDAnalyzer.Interfaces;
using EDAnalyzer.Models;
using ReactiveUI;
using Splat;

namespace EDAnalyzer.ViewModels
{
	public class MainViewModel : ReactiveObject, IMainViewModel
	{
		private readonly ObservableAsPropertyHelper<string> _commoditiesCount;
		private readonly ObservableAsPropertyHelper<bool> _indicatorSaving;
		private readonly ObservableAsPropertyHelper<int> _showingItemsCount;
		private readonly ObservableAsPropertyHelper<string> _stationCount;
		private readonly ObservableAsPropertyHelper<string> _systemCount;
		private readonly ReactiveList<Trade> _trades = new ReactiveList<Trade>();
		private string _filterString = "";
		private IReactiveDerivedList<ItemLine> _items;
		private IMainListViewModel _listViewModel;
		private ListSortDirection _orderDirection = ListSortDirection.Ascending;
		private string _orderField = "";
		private ItemLine _selectedItem;
		private Trade _selectedTrade;

		public MainViewModel(IScreen screen)
		{
			HostScreen = screen;
			ListViewModel = Locator.CurrentMutable.GetService<IMainListViewModel>();

			InterSystemCommand = InterSystemCommandFactory.Create(_trades, ListViewModel.AllItems);
			AllInterSystemsCommand = AllInterSystemsCommandFactory.Create(_trades, ListViewModel.AllItems);
			System15LyCommand = System15LyCommandFactory.Create(_trades, ListViewModel.AllItems);
			SaveAsyncCommand = ListViewModel.SaveAsync();
			FilterCommand = ReactiveCommand.Create();
			PurgeDataCommand = ReactiveCommand.Create();

			FilterCommand.Subscribe(_ => { FilterString = _ as string; });
			PurgeDataCommand.Subscribe(_ =>
			{
				var oldItems = ListViewModel.AllItems.Where(x => x.UpdatedAt < DateTime.Now.AddHours(-48)).ToList();
				oldItems.ForEach(x => ListViewModel.DeletionList.Add(x));
			});

			this.WhenAnyObservable(x => x.Items.CountChanged)
				.ToProperty(this, v => v.ShowingItemsCount, out _showingItemsCount);

			this.WhenAnyValue(x => x.ShowingItemsCount, x => x.ListViewModel.TotalItemsCount)
				.Select(count => string.Format("Items: {0} / {1}", count.Item1, count.Item2))
				.ToProperty(this, v => v.CommoditiesCount, out _commoditiesCount);

			this.WhenAnyObservable(x => x.ListViewModel.AllItems.ItemsAdded, x => x.ListViewModel.AllItems.ItemsRemoved)
				.Distinct(i => i.SystemName)
				.Select((line, count) => string.Format("Number of Systems: {0}", count))
				.ToProperty(this, v => v.SystemCount, out _systemCount);

			this.WhenAnyObservable(x => x.ListViewModel.AllItems.ItemsAdded, x => x.ListViewModel.AllItems.ItemsRemoved)
				.Distinct(i => i.StationName)
				.Select((line, count) => string.Format("Number of Stations: {0}", count))
				.ToProperty(this, v => v.StationCount, out _stationCount);

			this.WhenAny(x => x.ListViewModel.Saving, x => x.GetValue())
				.ToProperty(this, v => v.IndicatorSaving, out _indicatorSaving);

			Items = ListViewModel.AllItems.CreateDerivedCollection(
				x => x,
				ListFilter(),
				Compare(),
				this.WhenAny(x => x.FilterString, x => x.OrderField, x => x.OrderDirection,
					(filter, field, direction) => filter.Value));
		}

		public IMainListViewModel ListViewModel
		{
			get { return _listViewModel; }
			set { this.RaiseAndSetIfChanged(ref _listViewModel, value); }
		}

		public ItemLine SelectedItem
		{
			get { return _selectedItem; }
			set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
		}

		public bool IndicatorSaving
		{
			get { return _indicatorSaving.Value; }
		}

		public int ShowingItemsCount
		{
			get { return _showingItemsCount.Value; }
		}

		public string SystemCount
		{
			get { return _systemCount.Value; }
		}

		public string StationCount
		{
			get { return _stationCount.Value; }
		}

		public string CommoditiesCount
		{
			get { return _commoditiesCount.Value; }
		}

		public string FilterString
		{
			get { return _filterString; }
			set { this.RaiseAndSetIfChanged(ref _filterString, value); }
		}

		public ReactiveList<Trade> Trades
		{
			get { return _trades; }
		}

		public Trade SelectedTrade
		{
			get { return _selectedTrade; }
			set { this.RaiseAndSetIfChanged(ref _selectedTrade, value); }
		}

		public ReactiveCommand<Unit> SaveAsyncCommand { get; protected set; }
		public ReactiveCommand<object> PurgeDataCommand { get; protected set; }
		public ReactiveCommand<object> FilterCommand { get; protected set; }
		public ReactiveCommand<Unit> InterSystemCommand { get; protected set; }
		public ReactiveCommand<Unit> AllInterSystemsCommand { get; protected set; }
		public ReactiveCommand<Unit> System15LyCommand { get; protected set; }

		public string OrderField
		{
			get { return _orderField; }
			set { this.RaiseAndSetIfChanged(ref _orderField, value); }
		}

		public ListSortDirection OrderDirection
		{
			get { return _orderDirection; }
			set { this.RaiseAndSetIfChanged(ref _orderDirection, value); }
		}

		public IReactiveDerivedList<ItemLine> Items
		{
			get { return _items; }
			set { this.RaiseAndSetIfChanged(ref _items, value); }
		}

		public string UrlPathSegment
		{
			get { return "main"; }
		}

		public IScreen HostScreen { get; protected set; }

		private Func<ItemLine, ItemLine, int> Compare()
		{
			return (lineA, lineB) =>
				(OrderDirection == ListSortDirection.Ascending)
					? OrderedComparer<ItemLine>.OrderBy(x => x[OrderField]).Compare(lineA, lineB)
					: OrderedComparer<ItemLine>.OrderByDescending(x => x[OrderField]).Compare(lineA, lineB);
		}

		private Func<ItemLine, bool> ListFilter()
		{
			return x => (x.SystemName.ToLower().Contains(_filterString.ToLower())
			             || x.StationName.ToLower().Contains(_filterString.ToLower())
			             || x.CommodityName.ToLower().Contains(_filterString.ToLower())
			             || x.SellPrice.ToString().Contains(_filterString.ToLower())
			             || x.BuyPrice.ToString().Contains(_filterString.ToLower()));
		}
	}
}