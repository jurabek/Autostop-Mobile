using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Conditions;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	public abstract class BaseSearchPlaceViewModel : BaseViewModel, IBaseSearchPlaceViewModel
	{
		private readonly INavigationService _navigationService;
		private bool _isLoading;
		private string _searchText;
		private ObservableCollection<IAutoCompleteResultModel> _searchResults;
		private IAutoCompleteResultModel _selectedSearchResult;
		private EmptyAutocompleteResultModel _selectedEmptyResult;
		private string _placeholderText;

		protected BaseSearchPlaceViewModel(
			IPlacesProvider placesProvider,
			INavigationService navigationService)
		{
			placesProvider.Requires(nameof(placesProvider)).IsNotNull();
			navigationService.Requires(nameof(navigationService)).IsNotNull();

			_navigationService = navigationService;

			this.ObservablePropertyChanged(() => SearchText)
				.Throttle(TimeSpan.FromMilliseconds(300))
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(async searchText =>
				{
					try
					{
						IsSearching = true;
						if (string.IsNullOrEmpty(searchText))
							SearchResults = GetEmptyAutocompleteResult();
						else
							SearchResults = await placesProvider.GetAutoCompleteResponse(searchText);
						IsSearching = false;
					}
					catch (Exception)
					{
						SearchResults = null;
						IsSearching = false;
					}
				});

			this.ObservablePropertyChanged(() => SelectedSearchResult)
				.Where(r => r is EmptyAutocompleteResultModel)
				.Subscribe(selectedResult =>
				{
					if (selectedResult is HomeResultModel homeResultModel)
					{
						if (homeResultModel.Address == null)
						{
							navigationService.NavigateToSearchView<SearchHomeAddressViewModel>(vm =>
							{
								vm.GoBackCallback = GoBackCallback;
							});
						}
						else
						{
							SelectedEmptyResult = homeResultModel;
						}
					}
					else if (selectedResult is WorkResultModel workResultModel)
					{
						if (workResultModel.Address == null)
						{
							navigationService.NavigateToSearchView<SearchWorkAddressViewModel>(vm =>
							{
								vm.GoBackCallback = GoBackCallback;
							});
						}
						else
						{
							SelectedEmptyResult = workResultModel;
						}
					}
				});

			GoBack = new RelayCommand(() => navigationService.GoBack());
		}

		private void GoBackCallback()
		{
			_navigationService.GoBack();
			SearchResults = GetEmptyAutocompleteResult();
		}

		public bool IsSearching
		{
			get => _isLoading;
			set => RaiseAndSetIfChanged(ref _isLoading, value);
		}

		public string SearchText
		{
			get => _searchText;
			set => RaiseAndSetIfChanged(ref _searchText, value);
		}

		public string PlaceholderText
		{
			get => _placeholderText;
			set => RaiseAndSetIfChanged(ref _placeholderText, value);
		}

		public ObservableCollection<IAutoCompleteResultModel> SearchResults
		{
			get => _searchResults;
			set => RaiseAndSetIfChanged(ref _searchResults, value);
		}

		public IAutoCompleteResultModel SelectedSearchResult
		{
			get => _selectedSearchResult;
			set => RaiseAndSetIfChanged(ref _selectedSearchResult, value);
		}

		public EmptyAutocompleteResultModel SelectedEmptyResult
		{
			get => _selectedEmptyResult;
			set => RaiseAndSetIfChanged(ref _selectedEmptyResult, value);
		}

		public ICommand GoBack { get; }

		protected abstract ObservableCollection<IAutoCompleteResultModel> GetEmptyAutocompleteResult();
	}
}