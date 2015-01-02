using System.Windows;
using Akavache;
using EDAnalyzer.Interfaces;
using Splat;

namespace EDAnalyzer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void App_OnExit(object sender, ExitEventArgs e)
		{
			BlobCache.Shutdown().Wait();
			var eddnService = Locator.CurrentMutable.GetService<IEddnService>();
			eddnService.Disconnect();
			//var mainListVm = Locator.CurrentMutable.GetService<IMainListViewModel>();
			//mainListVm.SaveAsync().Execute(null);
		}
	}
}