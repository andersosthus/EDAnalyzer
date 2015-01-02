using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using EDAnalyzer.ViewModels;

namespace EDAnalyzer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			AppBootstrapper = new AppBootstrapper();
			DataContext = AppBootstrapper;
			Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
		}

		public AppBootstrapper AppBootstrapper { get; protected set; }

		//private void ImportCsv_Click(object sender, RoutedEventArgs e)
		//{
		//	var dlg = new OpenFileDialog
		//	{
		//		FileName = "",
		//		DefaultExt = ".csv",
		//		Filter = "Comma Separated (.csv)|*.csv"
		//	};

		//	var result = dlg.ShowDialog();
		//	if (result != true) return;

		//	var filename = dlg.FileName;
		//	using (var reader = new CsvReader(new StreamReader(File.OpenRead(filename))))
		//	{
		//		reader.Configuration.RegisterClassMap<UnifiedFormatMap>();
		//		reader.Configuration.Delimiter = ";";

		//		var records = reader.GetRecords<ItemLine>();
		//		foreach (var record in records)
		//		{
		//			//_items.Add(record);
		//		}
		//	}
		//}
	}
}