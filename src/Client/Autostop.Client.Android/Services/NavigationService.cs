using System;
using System.Reactive.Linq;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Android.Activities;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.IoC;
using Conditions;
using JetBrains.Annotations;
using Plugin.CurrentActivity;
using QueryTextChangeEventArgs = Android.Support.V7.Widget.SearchView.QueryTextChangeEventArgs;

namespace Autostop.Client.Android.Services
{
	[UsedImplicitly]
	public class NavigationService : INavigationService
	{
		private readonly ICurrentActivity _currentActivity;
		private readonly IViewAdapter<Fragment> _viewAdapter;
		private readonly IViewFactory _viewFactory;
		private IDisposable _searchTextChanged;
		private IDisposable _searchViewQueryChanged;

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

		public void NavigateTo<TViewModel>(bool root)
		{
			var viewModel = Locator.Resolve<TViewModel>();
			var fragment = GetFragment(viewModel);
			ReplaceContent(fragment, root);
		}

		public void NavigateTo<TViewModel>(TViewModel viewModel)
		{
			var fragment = GetFragment(viewModel);
			ReplaceContent(fragment);
		}

		public void NavigateToModal<TViewModel>()
		{
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
			var viewModel = Locator.Resolve<TViewModel>();
			ShowSearchView(viewModel);
			var fragment = GetFragment(viewModel);
			callBack?.Invoke(viewModel);
			ReplaceContent(fragment);
		}

		public void NavigateToSearchView<TViewModel>(TViewModel viewModel) where TViewModel : ISearchableViewModel
		{
			ShowSearchView(viewModel);
			var fragment = GetFragment(viewModel);
			ReplaceContent(fragment);
		}

		private void ShowSearchView(ISearchableViewModel searchableViewModel)
		{
			var mainActivity = (MainActivity)_currentActivity.Activity;
			mainActivity.TitleTextView.Visibility = ViewStates.Gone;

			var searchView = mainActivity.SearchView;
			searchView.Visibility = ViewStates.Visible;
			searchView.QueryHint = searchableViewModel.PlaceholderText;

			_searchTextChanged = searchableViewModel
				.Changed(() => searchableViewModel.SearchText)
				.Subscribe(searchText => searchView.SetQuery(searchText, false));

			_searchViewQueryChanged = Observable.FromEventPattern<EventHandler<QueryTextChangeEventArgs>, QueryTextChangeEventArgs>(
						e => searchView.QueryTextChange += e,
						e => searchView.QueryTextChange -= e)
				.Subscribe(ep => searchableViewModel.SearchText = ep.EventArgs.NewText);

			searchableViewModel.SearchText = string.Empty;

			searchView.RequestFocus();
			InputMethodManager imm = (InputMethodManager)mainActivity.GetSystemService(Context.InputMethodService);
			imm.ShowSoftInput(searchView, ShowFlags.Implicit);
		}

		public void GoBack()
		{
			_searchTextChanged?.Dispose();
			_searchViewQueryChanged?.Dispose();
			var _ = _currentActivity.Activity.FragmentManager.PopBackStackImmediate();
		}

		public void NavigateToRoot()
		{
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

		private int ReplaceContent(Fragment fragment, bool root = false)
		{
			fragment.Requires(nameof(fragment)).IsNotNull();

			var fragmentManager = _currentActivity.Activity.FragmentManager;

			var transaction = fragmentManager.BeginTransaction();
			//transaction.SetCustomAnimations(Resource.Animation.enter_from_left, Resource.Animation.exit_to_right);

			return root ? 
				transaction
					.Replace(Resource.Id.container, fragment)
					.Commit() : 
				transaction
					.Replace(Resource.Id.container, fragment)
					.AddToBackStack(null)
					.Commit();
		}
	}
}