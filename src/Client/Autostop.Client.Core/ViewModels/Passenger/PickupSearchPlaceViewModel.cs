using Autostop.Client.Abstraction.Providers;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public class PickupSearchPlaceViewModel : BaseSearchPlaceViewModel
    {
        public PickupSearchPlaceViewModel(IPlacesProvider placesProvider) : base(placesProvider)
        {
        }
    }
}