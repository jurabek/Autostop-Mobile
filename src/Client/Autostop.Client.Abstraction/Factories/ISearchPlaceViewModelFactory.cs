using Autostop.Client.Abstraction.ViewModels;

namespace Autostop.Client.Abstraction.Factories
{
    public interface ISearchPlaceViewModelFactory
    {
        IBaseLocationEditorViewModel GetDestinationLocationEditorViewModel(ITripLocationViewModel tripLocationViewModel);
        IBaseLocationEditorViewModel GetPickupLocationEditorViewModel();
    }
}