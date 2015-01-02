using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;

namespace EDAnalyzer.ViewModels
{
	public class EddnViewModel : ReactiveObject
	{
		public EddnViewModel()
		{
			PublishCommand = ReactiveCommand.Create();
			MessageBus.Current.RegisterMessageSource(PublishCommand);

			RxApp.MainThreadScheduler = new DispatcherScheduler(Application.Current.Dispatcher);


			Task.Factory.StartNew(() => Debug.WriteLine("Starting recieving"));
		}

		public IReactiveCommand<object> PublishCommand { get; protected set; }
	}
}