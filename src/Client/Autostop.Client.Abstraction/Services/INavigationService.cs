using System;

namespace Autostop.Client.Abstraction.Services
{
    public interface INavigationService
    {
        void NavigateTo(Type viewModelType);

        void NavigateTo(Type viewModelType, Action<object> configure);

        void NavigateTo<TViewModel>();

        void NavigateTo<TViewModel>(Action<object, TViewModel> configure);

        void GoBack();
    }
}