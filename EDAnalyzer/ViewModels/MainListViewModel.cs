using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EDAnalyzer.Interfaces;
using EDAnalyzer.Models;
using ProtoBuf;
using ReactiveUI;
using Splat;

namespace EDAnalyzer.ViewModels
{
	public class MainListViewModel : ReactiveObject, IMainListViewModel
	{
		private readonly ReactiveList<ItemLine> _completeList = new ReactiveList<ItemLine>();
		private readonly IEddnService _eddnService;
		private readonly ReactiveList<ItemLine> _processorList = new ReactiveList<ItemLine>();
		private readonly ISaveService _saveService;
		private readonly SHA256 _sha;
		private readonly ObservableAsPropertyHelper<int> _totalItemsCount;
		private ReactiveList<ItemLine> _deletionList = new ReactiveList<ItemLine>();
		private EddnViewModel _eddnViewModel;
		private bool _saving;

		public MainListViewModel()
		{
			_saveService = Locator.Current.GetService<ISaveService>();
			_eddnService = Locator.Current.GetService<IEddnService>();
			var cts = new CancellationTokenSource();
			_completeList.ChangeTrackingEnabled = true;
			_sha = SHA256.Create();

			_eddnService.FetchFromEddnAsync(cts.Token)
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.SubscribeOn(TaskPoolScheduler.Default)
				.Subscribe(i => _processorList.Add(i));

			LoadItemsFromDisk = ReactiveCommand.CreateAsyncObservable(_ => LoadFromDisk());
			LoadItemsFromDisk
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.SubscribeOn(TaskPoolScheduler.Default)
				.Subscribe(i => _processorList.Add(i));

			LoadItemsFromDisk.ThrownExceptions.Subscribe(ex => Debug.WriteLine("ERROR"));
			LoadItemsFromDisk.ExecuteAsyncTask();

			_processorList.ItemsAdded
				.Select(ComputeItemHash)
				.Subscribe(RemoveDuplicatesAndAddItemToList);

			_deletionList.ItemsAdded
				.Subscribe(x =>
				{
					_completeList.Remove(x);
					_deletionList.Remove(x);
				});

			this.WhenAnyObservable(x => x._completeList.CountChanged)
				.ToProperty(this, v => v.TotalItemsCount, out _totalItemsCount);

			this.WhenAnyObservable(x => x._completeList.ItemsAdded, x => x._completeList.ItemsRemoved)
				.Throttle(TimeSpan.FromSeconds(2))
				.InvokeCommand(SaveAsync());
		}

		public EddnViewModel EddnViewModel
		{
			get { return _eddnViewModel; }
			set { this.RaiseAndSetIfChanged(ref _eddnViewModel, value); }
		}

		public ReactiveCommand<ItemLine> LoadItemsFromDisk { get; protected set; }

		public IReadOnlyReactiveList<ItemLine> AllItems
		{
			get { return _completeList; }
		}

		public ReactiveList<ItemLine> DeletionList
		{
			get { return _deletionList; }
			set { this.RaiseAndSetIfChanged(ref _deletionList, value); }
		}

		public bool Saving
		{
			get { return _saving; }
			set { this.RaiseAndSetIfChanged(ref _saving, value); }
		}

		public int TotalItemsCount
		{
			get { return _totalItemsCount.Value; }
		}

		public ReactiveCommand<Unit> SaveAsync()
		{
			return ReactiveCommand.CreateAsyncTask(async _ =>
			{
				if (!Directory.Exists(Constants.DataPath))
					Directory.CreateDirectory(Constants.DataPath);

				Saving = true;
				await _saveService.SaveCommodities(_completeList);
				await Task.Delay(TimeSpan.FromMilliseconds(200));
				Saving = false;
			});
		}

		private void RemoveDuplicatesAndAddItemToList(ItemLine line)
		{
			var duplicates = _completeList.Where(i => i.Hash.Equals(line.Hash)).ToList();
			if (!duplicates.Any())
				_completeList.Add(line);
			else
			{
				foreach (var duplicate in duplicates.Where(duplicate => line.UpdatedAt > duplicate.UpdatedAt))
				{
					_completeList.Remove(duplicate);
					_completeList.Add(line);
				}
			}
			_processorList.Remove(line);
		}

		private ItemLine ComputeItemHash(ItemLine item)
		{
			if (!string.IsNullOrEmpty(item.Hash)) return item;

			var hashBase = string.Format("{0}_{1}_{2}", item.SystemName.ToLower(), item.StationName.ToLower(),
				item.CommodityName.ToLower());
			var bytes = Encoding.UTF8.GetBytes(hashBase);
			using (var ms = new MemoryStream(bytes))
			{
				var byteHash = _sha.ComputeHash(ms);
				item.Hash = HashToString(byteHash);
			}

			return item;
		}

		private static string HashToString(IList<byte> array)
		{
			var hash = new StringBuilder(64);
			int i;
			for (i = 0; i < array.Count; i++)
			{
				hash.AppendFormat("{0:X2}", array[i]);
			}

			return hash.ToString();
		}

		private static IObservable<ItemLine> LoadFromDisk()
		{
			return Observable.Create<ItemLine>(async observer =>
			{
				if (!Directory.Exists(Constants.DataPath))
					Directory.CreateDirectory(Constants.DataPath);

				if (!File.Exists(Constants.DataFile))
					observer.OnCompleted();

				using (var file = File.OpenRead(Constants.DataFile))
				{
					var items = await Task.Factory.StartNew(() => Serializer.Deserialize<List<ItemLine>>(file));
					items.ForEach(observer.OnNext);
				}

				observer.OnCompleted();
			});
		}
	}
}