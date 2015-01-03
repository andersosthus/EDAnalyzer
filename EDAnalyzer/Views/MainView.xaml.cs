using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EDAnalyzer.Interfaces;
using ReactiveUI;

namespace EDAnalyzer.Views
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : IViewFor<IMainViewModel>
	{
		public static readonly DependencyProperty ViewModelProperty =
			DependencyProperty.Register("ViewModel", typeof (IMainViewModel), typeof (MainView), new PropertyMetadata(null));

		private GridViewColumnHeader _prevHeaderSorted;
		private string _sortHeader;
		private ListSortDirection _sortOrder = ListSortDirection.Ascending;

		public MainView()
		{
			InitializeComponent();
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IMainViewModel) value; }
		}

		public IMainViewModel ViewModel
		{
			get { return (IMainViewModel) GetValue(ViewModelProperty); }
			set
			{
				SetValue(ViewModelProperty, value);
				DataContext = value;
			}
		}

		private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
		{
			var headerClicked = e.OriginalSource as GridViewColumnHeader;
			if (headerClicked == null) return;
			if (headerClicked.Role == GridViewColumnHeaderRole.Padding) return;

			var headerName = headerClicked.Column.Header as string;
			if (string.IsNullOrEmpty(headerName)) return;

			if (headerName.Equals(_sortHeader))
			{
				_sortOrder = ReverseSortOrder();
				ViewModel.OrderDirection = _sortOrder;
				SetColumnHeaderArrow(headerClicked);
				return;
			}

			_sortOrder = ListSortDirection.Ascending;
			var vmSortHeader = string.Empty;

			switch (headerName)
			{
				case "System Name":
					vmSortHeader = "systemname";
					break;
				case "Station Name":
					vmSortHeader = "stationname";
					break;
				case "Commodity Name":
					vmSortHeader = "commodityname";
					break;
				case "Buy Price":
					vmSortHeader = "buyprice";
					break;
				case "Sell Price":
					vmSortHeader = "sellprice";
					break;
				case "Demand":
					vmSortHeader = "demand";
					break;
				case "Updated At":
					vmSortHeader = "updatedat";
					break;
			}

			SetColumnHeaderArrow(headerClicked);
			_sortHeader = headerName;
			_prevHeaderSorted = headerClicked;
			ViewModel.OrderField = vmSortHeader;
			ViewModel.OrderDirection = _sortOrder;
		}

		private void SetColumnHeaderArrow(GridViewColumnHeader headerClicked)
		{
			if (_prevHeaderSorted != null && !Equals(_prevHeaderSorted, headerClicked))
				_prevHeaderSorted.Column.HeaderTemplate = null;

			if (_sortOrder == ListSortDirection.Ascending)
				headerClicked.Column.HeaderTemplate = Application.Current.FindResource("HeaderTemplateArrowUp") as DataTemplate;
			else
				headerClicked.Column.HeaderTemplate = Application.Current.FindResource("HeaderTemplateArrowDown") as DataTemplate;
		}

		private ListSortDirection ReverseSortOrder()
		{
			return _sortOrder == ListSortDirection.Ascending
				? ListSortDirection.Descending
				: ListSortDirection.Ascending;
		}

		private void TradeListColumnHeader_OnClick(object sender, RoutedEventArgs e)
		{
		}
	}
}