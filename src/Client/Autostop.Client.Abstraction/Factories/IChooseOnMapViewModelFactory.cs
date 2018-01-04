using Autostop.Client.Abstraction.ViewModels;

namespace Autostop.Client.Abstraction.Factories
{
    public interface IChooseOnMapViewModelFactory
    {
        ISearchableViewModel GetChooseDestinationOnMapViewModel(IRideViewModel rideViewModel);
    }
}