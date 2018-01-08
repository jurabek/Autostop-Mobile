using System;
using Android.App;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.IoC;
using JetBrains.Annotations;
using Plugin.CurrentActivity;

namespace Autostop.Client.Android.Services
{
	[UsedImplicitly]
	public class NavigationService : INavigationService
	{
		private readonly ICurrentActivity _currentActivity;
		private readonly IViewAdapter<Fragment> _viewAdapter;
	    private readonly IViewFactory _viewFactory;

        public NavigationService(
			ICurrentActivity currentActivity,
            IViewAdapter<Fragment> viewAdapter, 
            IViewFactory viewFactory)
        {
	        _currentActivity = currentActivity;
	        _viewAdapter = viewAdapter;
            _viewFactory = viewFactory;
        }
		public void NavigateTo(Type viewModelType)
		{
		    var fragment = GetFragment(viewModelType);
            ReplaceContent(fragment);
		}

		public void NavigateTo(Type viewModelType, Action<object> configure)
		{
		    var fragment = GetFragment(viewModelType);
		    configure(fragment);
            ReplaceContent(fragment);
		}

        public void NavigateTo<TViewModel>()
        {
            var viewModel = Locator.Resolve<TViewModel>();
            var fragment = GetFragment(viewModel);
            ReplaceContent(fragment);
        }

		public void NavigateTo<TViewModel>(TViewModel viewModel)
		{
		    var fragment = GetFragment(viewModel);
		    ReplaceContent(fragment);
        }

		public void NavigateToModal<TViewModel>()
		{
			throw new NotImplementedException();
		}

		public void NavigateTo<TViewModel>(Action<object, TViewModel> configure)
		{
		    var viewModel = Locator.Resolve<TViewModel>();
		    var fragment = GetFragment(viewModel);
		    configure(fragment, viewModel);
		    ReplaceContent(fragment);
        }

		public void NavigateToSearchView<TViewModel>(Action<TViewModel> callBack) where TViewModel : ISearchableViewModel
		{
			throw new NotImplementedException();
		}

		public void NavigateToSearchView<TViewModel>(TViewModel viewModel) where TViewModel : ISearchableViewModel
		{
			throw new NotImplementedException();
		}

		public void GoBack()
		{
			_currentActivity.Activity.FragmentManager.PopBackStack();
		}

		public void NavigaeToRoot()
		{
			throw new NotImplementedException();
		}

	    private Fragment GetFragment<TViewModel>(TViewModel viewModel)
	    {
	        var view = _viewFactory.CreateView(viewModel);
	        var fragment = _viewAdapter.GetView(view);
	        return fragment;
	    }

        private Fragment GetFragment(Type viewModelType)
	    {
	        var viewModel = Locator.Resolve(viewModelType);
	        var createView = typeof(IViewFactory).GetMethod(nameof(IViewFactory.CreateView))
	            ?.MakeGenericMethod(viewModelType);
	        var screenFor = createView?.Invoke(this, new[] { viewModel });
	        var getView = typeof(IViewAdapter<Fragment>)
	            .GetMethod(nameof(IViewAdapter<Fragment>.GetView))
	            ?.MakeGenericMethod(viewModelType);

	        var fragment = getView?.Invoke(this, new[] { screenFor }) as Fragment;
	        return fragment;
	    }

	    private void ReplaceContent(Fragment fragment)
	    {
	        var fragmentManager = _currentActivity.Activity.FragmentManager;

	        if (fragmentManager.BackStackEntryCount > 0)
	        {
	            fragmentManager
	                .BeginTransaction()
	                .Replace(Resource.Id.container, fragment)
	                .AddToBackStack(null)
	                .Commit();
	        }
	        else
	        {
	            fragmentManager
	                .BeginTransaction()
	                .Replace(Resource.Id.container, fragment)
	                .Commit();
	        }
	    }
    }
}