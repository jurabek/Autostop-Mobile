using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;

namespace Autostop.Client.Abstraction.Factories
{
    public interface ISearchPlaceViewModelFactory
    {
        IBaseSearchPlaceViewModel DestinationSearchPlaceViewModel(IRideViewModel rideViewModel);
        IBaseSearchPlaceViewModel GetPickupSearchPlaceViewModel();
    }
}