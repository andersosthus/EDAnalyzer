using System.Windows;
using Akavache;

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
			//var mainListVm = Locator.CurrentMutable.GetService<IMainListViewModel>();
			//mainListVm.SaveAsync().Execute(null);
		}
	}
}