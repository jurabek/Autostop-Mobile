using Autostop.Client.Abstraction.ViewModels;

namespace Autostop.Client.Abstraction.Adapters
{
    public interface IViewAdapter<out TNativeView> where TNativeView : class
    {
        TNativeView GetView<TViewModel>(IScreenFor<TViewModel> view);

	    TNativeView GetSearchView<TViewModel>(IScreenFor<TViewModel> view) where TViewModel : ISearchableViewModel;
    }
}