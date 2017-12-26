using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Extensions;
using JetBrains.Annotations;
using System;
using System.Reactive.Linq;
using Autostop.Client.Core.Models;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
    [UsedImplicitly]
    public class DestinationSearchPlaceViewModel : BaseSearchPlaceViewModel
    {
	    private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

	    public DestinationSearchPlaceViewModel(
			INavigationService navigationService,
			IPlacesProvider placesProvider,
			IGeocodingProvider geocodingProvider,
			IEmptyAutocompleteResultProvider autocompleteResultProvider) : base(placesProvider, geocodingProvider, navigationService)
	    {
		    _autocompleteResultProvider = autocompleteResultProvider;
		    PlaceholderText = "Set destination location";

		    this.ObservablePropertyChanged(() => SelectedSearchResult)
				.Where(r => r is SetLocationOnMapResultModel)
			    .Subscribe(r =>
			    {
					navigationService.NavigateTo<ChooseDestinationOnMapViewModel>();
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
    }
}