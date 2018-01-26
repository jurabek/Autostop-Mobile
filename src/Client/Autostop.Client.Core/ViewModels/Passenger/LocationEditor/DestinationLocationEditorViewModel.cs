using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap;

namespace Autostop.Client.Core.ViewModels.Passenger.LocationEditor
{
    public sealed class DestinationLocationEditorViewModel : BaseLocationEditorViewModel
	{
		private readonly ITripLocationViewModel _tripLocationViewModel;
	    private readonly INavigationService _navigationService;
	    private readonly IChooseOnMapViewModelFactory _chooseOnMapViewModelFactory;
	    private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

        public DestinationLocationEditorViewModel(
			ISchedulerProvider schedulerProvider,
            ITripLocationViewModel tripLocationViewModel,
            INavigationService navigationService,
            IPlacesProvider placesProvider,
            IGeocodingProvider geocodingProvider,
            IChooseOnMapViewModelFactory chooseOnMapViewModelFactory,
            IEmptyAutocompleteResultProvider autocompleteResultProvider) : base(schedulerProvider, placesProvider, geocodingProvider, navigationService)
        {
	        _tripLocationViewModel = tripLocationViewModel;
	        _navigationService = navigationService;
	        _chooseOnMapViewModelFactory = chooseOnMapViewModelFactory;
	        _autocompleteResultProvider = autocompleteResultProvider;

			this.Changed(() => SelectedSearchResult)
                .Where(r => r is SetLocationOnMapResultModel)
                .Subscribe(NavigateToChooseDestinationOnMapViewModel);

	        SelectedEmptyAutocompleteResultModelObservable()
				.Where(r => r.Address == null)
				.Subscribe(SelectedEmptyAutocompleteResultModel); 
		}

	    private void NavigateToChooseDestinationOnMapViewModel(IAutoCompleteResultModel autoCompleteResultModel)
	    {
		    var chooseDestinationOnMapViewModel = _chooseOnMapViewModelFactory.GetChooseDestinationOnMapViewModel(_tripLocationViewModel);
		    _navigationService.NavigateTo(chooseDestinationOnMapViewModel as ChooseDestinationOnMapViewModel);
		}

	    private void SelectedEmptyAutocompleteResultModel(IAutoCompleteResultModel selectedResult)
	    {
		    switch (selectedResult)
		    {
			    case HomeResultModel _:
				    _navigationService.NavigateToSearchView<HomeLocationEditorViewModel>(vm => vm.GoBackCallback = GoBackCallback);
				    break;
			    case WorkResultModel _:
				    _navigationService.NavigateToSearchView<WorkLocationEditorViewModel>(vm => vm.GoBackCallback = GoBackCallback);
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