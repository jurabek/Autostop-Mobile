using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using JetBrains.Annotations;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	[UsedImplicitly]
    public sealed class DestinationSearchPlaceViewModel : BaseSearchPlaceViewModel
	{
		private readonly IRideViewModel _rideViewModel;
	    private readonly INavigationService _navigationService;
	    private readonly IChooseOnMapViewModelFactory _chooseOnMapViewModelFactory;
	    private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

        public DestinationSearchPlaceViewModel(
			ISchedulerProvider schedulerProvider,
            IRideViewModel rideViewModel,
            INavigationService navigationService,
            IPlacesProvider placesProvider,
            IGeocodingProvider geocodingProvider,
            IChooseOnMapViewModelFactory chooseOnMapViewModelFactory,
            IEmptyAutocompleteResultProvider autocompleteResultProvider) : base(schedulerProvider, placesProvider, geocodingProvider, navigationService)
        {
	        _rideViewModel = rideViewModel;
	        _navigationService = navigationService;
	        _chooseOnMapViewModelFactory = chooseOnMapViewModelFactory;
	        _autocompleteResultProvider = autocompleteResultProvider;

			this.ObservablePropertyChanged(() => SelectedSearchResult)
                .Where(r => r is SetLocationOnMapResultModel)
                .Subscribe(NavigateToChooseDestinationOnMapViewModel);

	        SelectedEmptyAutocompleteResultModel()
				.Where(r => r.Address == null)
				.Subscribe(SelectedEmptyAutocompleteResultModel); 
		}

	    private void NavigateToChooseDestinationOnMapViewModel(IAutoCompleteResultModel autoCompleteResultModel)
	    {
		    var chooseDestinationOnMapViewModel = _chooseOnMapViewModelFactory.GetChooseDestinationOnMapViewModel(_rideViewModel);
		    _navigationService.NavigateTo(chooseDestinationOnMapViewModel as ChooseDestinationOnMapViewModel);
		}

	    private void SelectedEmptyAutocompleteResultModel(IAutoCompleteResultModel selectedResult)
	    {
		    switch (selectedResult)
		    {
			    case HomeResultModel _:
				    _navigationService.NavigateToSearchView<SearchHomeAddressViewModel>(vm => vm.GoBackCallback = GoBackCallback);
				    break;
			    case WorkResultModel _:
				    _navigationService.NavigateToSearchView<SearchWorkAddressViewModel>(vm => vm.GoBackCallback = GoBackCallback);
				    break;
		    }
	    }

	    private void GoBackCallback()
	    {
		    _navigationService.GoBack();
		    SearchResults = GetEmptyAutocompleteResult();
	    }

		protected override ObservableCollection<IAutoCompleteResultModel> GetEmptyAutocompleteResult()
        {
            return new ObservableCollection<IAutoCompleteResultModel>
            {
                _autocompleteResultProvider.GetHomeResultModel(),
                _autocompleteResultProvider.GetWorkResultModel(),
                _autocompleteResultProvider.GetSetLocationOnMapResultModel()
            };
        }

        public override string PlaceholderText => "Set destination location";
    }
}