using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Extensions;
using JetBrains.Annotations;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.Models;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
    [UsedImplicitly]
    public sealed class DestinationSearchPlaceViewModel : BaseSearchPlaceViewModel
    {
	    private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

        public DestinationSearchPlaceViewModel(
			IScheduler scheduler,
            IRideViewModel rideViewModel,
			INavigationService navigationService,
			IPlacesProvider placesProvider,
			IGeocodingProvider geocodingProvider,
            IChooseOnMapViewModelFactory chooseOnMapViewModelFactory,
			IEmptyAutocompleteResultProvider autocompleteResultProvider) : base(scheduler, placesProvider, geocodingProvider, navigationService)
	    {
	        _autocompleteResultProvider = autocompleteResultProvider;
			
		    this.ObservablePropertyChanged(() => SelectedSearchResult)
				.Where(r => r is SetLocationOnMapResultModel)
			    .Subscribe(r =>
			    {
				    var chooseDestinationOnMapViewModel =
					    chooseOnMapViewModelFactory.GetChooseDestinationOnMapViewModel(rideViewModel) as
						    ChooseDestinationOnMapViewModel;
					navigationService.NavigateTo(chooseDestinationOnMapViewModel);
			    });
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