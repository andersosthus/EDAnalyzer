using EDAnalyzer.Interfaces;
using EDAnalyzer.Services;
using EDAnalyzer.Views;
using ReactiveUI;
using Splat;

namespace EDAnalyzer.ViewModels
{
	public class AppBootstrapper : ReactiveObject, IScreen
	{
		public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState testRouter = null)
		{
			Router = testRouter ?? new RoutingState();
			dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

			RegisterParts(dependencyResolver);

			LogHost.Default.Level = LogLevel.Debug;

			Router.Navigate.Execute(new MainViewModel(this));
		}

		public RoutingState Router { get; private set; }

		private void RegisterParts(IMutableDependencyResolver dependencyResolver)
		{
			dependencyResolver.RegisterConstant(this, typeof (IScreen));
			dependencyResolver.Register(() => new EddnService(), typeof (IEddnService));
			dependencyResolver.Register(() => new MainView(), typeof (IViewFor<MainViewModel>));
			dependencyResolver.Register(() => new MainListViewModel(), typeof (IMainListViewModel));
			dependencyResolver.Register(() => new SaveService(), typeof (ISaveService));
			dependencyResolver.Register(() => new CalculateLootService(), typeof (ICalculateLoot));
			dependencyResolver.Register(() => new EdscService(), typeof (IQueryEdsc));
		}
	}
}