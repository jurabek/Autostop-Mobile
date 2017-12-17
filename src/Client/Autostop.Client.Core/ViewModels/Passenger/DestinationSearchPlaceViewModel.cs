using Autostop.Client.Abstraction.Providers;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public class DestinationSearchPlaceViewModel : BaseSearchPlaceViewModel
    {
	    public DestinationSearchPlaceViewModel(IPlacesProvider placesProvider) : base(placesProvider)
	    {
	    }
    }
}
