using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
    [UsedImplicitly]
    public class DestinationSearchPlaceViewModel : BaseSearchPlaceViewModel
    {
	    private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

	    public DestinationSearchPlaceViewModel(
			INavigationService navigationService,
			IPlacesProvider placesProvider, 
			IEmptyAutocompleteResultProvider autocompleteResultProvider) : base(placesProvider, navigationService)
	    {
		    _autocompleteResultProvider = autocompleteResultProvider;
		    PlaceholderText = "Set destination location";
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