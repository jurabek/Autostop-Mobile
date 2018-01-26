using Autostop.Client.Abstraction.ViewModels;

namespace Autostop.Client.Abstraction.Factories
{
    public interface ISearchPlaceViewModelFactory
    {
        IBaseSearchPlaceViewModel DestinationSearchPlaceViewModel(ITripLocationViewModel tripLocationViewModel);
        IBaseSearchPlaceViewModel GetPickupSearchPlaceViewModel();
    }
}