using System;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;

namespace Autostop.Client.Abstraction.Services
{
    public interface INavigationService
    {
        void NavigateTo(Type viewModelType);

        void NavigateTo(Type viewModelType, Action<object> configure);

        void NavigateTo<TViewModel>();

        void NavigateTo<TViewModel>(TViewModel viewModel);

        void NavigateToModal<TViewModel>();

		void NavigateTo<TViewModel>(Action<object, TViewModel> configure);

	    void NavigateToSearchView<TViewModel>(Action<TViewModel> callBack) where TViewModel : ISearchableViewModel;

	    void NavigateToSearchView<TViewModel>(TViewModel viewModel) where TViewModel : ISearchableViewModel;

		void GoBack();
    }
}