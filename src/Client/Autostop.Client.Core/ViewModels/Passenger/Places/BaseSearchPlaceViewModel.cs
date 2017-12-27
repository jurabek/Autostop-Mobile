using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Autostop.Common.Shared.Models;
using Conditions;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	public abstract class BaseSearchPlaceViewModel : BaseViewModel, IBaseSearchPlaceViewModel
	{
	    private readonly Subject<Address> _selectedAddress = new Subject<Address>();
        private readonly IGeocodingProvider _geocodingProvider;
		private readonly INavigationService _navigationService;
		private ObservableCollection<IAutoCompleteResultModel> _searchResults;
		private IAutoCompleteResultModel _selectedSearchResult;
	    private bool _isLoading;
	    private string _searchText;

        protected BaseSearchPlaceViewModel(
			IPlacesProvider placesProvider,
			IGeocodingProvider geocodingProvider,
			INavigationService navigationService)
		{
			placesProvider.Requires(nameof(placesProvider)).IsNotNull();
			navigationService.Requires(nameof(navigationService)).IsNotNull();

			_geocodingProvider = geocodingProvider;
			_navigationService = navigationService;
			SelectedAddress = _selectedAddress;

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
				.Subscribe(SelectedEmptyAutocompleteResultModel);

			this.ObservablePropertyChanged(() => SelectedSearchResult)
				.Where(r => r is AutoCompleteResultModel)
				.Select(async result => await SelectedAutocompleteResultModel(result))
				.Subscribe();

			GoBack = new RelayCommand(() => navigationService.GoBack());
		}

		private async Task SelectedAutocompleteResultModel(IAutoCompleteResultModel result)
		{
			var address = await _geocodingProvider.ReverseGeocodingFromPlaceId(result.PlaceId);
			_selectedAddress.OnNext(address);
		}

		private void SelectedEmptyAutocompleteResultModel(IAutoCompleteResultModel selectedResult)
		{
			if (selectedResult is HomeResultModel homeResultModel)
			{
				if (homeResultModel.Address == null)
					_navigationService.NavigateToSearchView<SearchHomeAddressViewModel>(vm => vm.GoBackCallback = GoBackCallback);
				else
					_selectedAddress.OnNext(homeResultModel.Address);
			}
			else if (selectedResult is WorkResultModel workResultModel)
			{
				if (workResultModel.Address == null)
					_navigationService.NavigateToSearchView<SearchWorkAddressViewModel>(vm => vm.GoBackCallback = GoBackCallback);
				else
					_selectedAddress.OnNext(workResultModel.Address);
			}
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

		public virtual string SearchText
		{
			get => _searchText;
			set => RaiseAndSetIfChanged(ref _searchText, value);
		}

	    public virtual string PlaceholderText => "Search";

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

		public IObservable<Address> SelectedAddress { get; }

		public virtual ICommand GoBack { get; }

		protected abstract ObservableCollection<IAutoCompleteResultModel> GetEmptyAutocompleteResult();
	}
}