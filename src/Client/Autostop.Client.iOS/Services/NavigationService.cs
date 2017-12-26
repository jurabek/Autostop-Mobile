using System;
using Autofac;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.Core;
using UIKit;

namespace Autostop.Client.iOS.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IContainer _container = BootstrapperBase.Container;
        private readonly UINavigationController _navigationController;
        private readonly IViewAdapter<UIViewController> _viewAdapter;
        private readonly IViewFactory _viewFactory;

        public NavigationService(
            IViewFactory viewFactory,
            IViewAdapter<UIViewController> viewAdapter)
        {
            _viewFactory = viewFactory;
            _viewAdapter = viewAdapter;
            _navigationController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
        }

        public void NavigateTo(Type viewModelType)
        {
            var viewController = GetViewController(viewModelType);
            _navigationController.PushViewController(viewController as UIViewController, false);
        }

        public void NavigateTo(Type viewModelType, Action<object> configure)
        {
            var viewController = GetViewController(viewModelType);
            configure(viewController);
            _navigationController.PushViewController(viewController as UIViewController, false);
        }

        public void NavigateTo<TViewModel>()
        {
            var viewModel = _container.Resolve<TViewModel>();
            var viewController = GetViewController(viewModel);
            _navigationController.PushViewController(viewController, false);
        }

        public void NavigateTo<TViewModel>(Action<object, TViewModel> configure)
        {
            var viewModel = _container.Resolve<TViewModel>();
            var viewController = GetViewController(viewModel);
            configure(viewController, viewModel);
            _navigationController.PushViewController(viewController, false);
        }

	    public void NavigateToSearchView<TViewModel>(Action<TViewModel> callBack) where TViewModel : IBaseSearchPlaceViewModel
	    {
		    var viewModel = _container.Resolve<TViewModel>();
			var view = _viewFactory.CreateView(viewModel);
		    var viewController = _viewAdapter.GetSearchView(view);
		    callBack(viewModel);
			_navigationController.PushViewController(viewController, false);
		}

		public void GoBack()
        {
            _navigationController.PopViewController(false);
        }

        private UIViewController GetViewController<TViewModel>(TViewModel viewModel)
        {
            var view = _viewFactory.CreateView(viewModel);
            var viewController = _viewAdapter.GetView(view);
            return viewController;
        }

        private object GetViewController(Type viewModelType)
        {
            var viewModel = _container.Resolve(viewModelType);
            var createView = typeof(IViewFactory).GetMethod(nameof(IViewFactory.CreateView))
                ?.MakeGenericMethod(viewModelType);
            var view = createView?.Invoke(this, new[] {viewModel});
            var getView = typeof(IViewAdapter<UIViewController>)
                .GetMethod(nameof(IViewAdapter<UIViewController>.GetView))
                ?.MakeGenericMethod(viewModelType);

            var viewController = getView?.Invoke(this, new[] {view});
            return viewController;
        }
    }
}