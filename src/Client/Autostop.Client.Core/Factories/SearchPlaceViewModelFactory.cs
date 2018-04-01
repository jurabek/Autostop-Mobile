using Autofac;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.IoC;
using ViewModelKeys = Autostop.Common.Shared.Constants.IoC.ViewModelKeys;

namespace Autostop.Client.Core.Factories
{
    public class SearchPlaceViewModelFactory : ISearchPlaceViewModelFactory
    {
        public IBaseLocationEditorViewModel GetPickupLocationEditorViewModel()
        {
            return Locator.ResolveNamed<IBaseLocationEditorViewModel>(ViewModelKeys.PickupSearch);
        }

        public IBaseLocationEditorViewModel GetDestinationLocationEditorViewModel()
        {
            return Locator.ResolveNamed<IBaseLocationEditorViewModel>(ViewModelKeys.DestinationSearch);
        }
    }
}
