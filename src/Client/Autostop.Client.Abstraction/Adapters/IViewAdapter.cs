using Autostop.Client.Abstraction.ViewModels.Passenger.Places;

namespace Autostop.Client.Abstraction.Adapters
{
    public interface IViewAdapter<out TView> where TView : class
    {
        TView GetView<TViewModel>(IScreenFor<TViewModel> view);

	    TView GetSearchView<TViewModel>(IScreenFor<TViewModel> view) where TViewModel : IBaseSearchPlaceViewModel;
    }
}