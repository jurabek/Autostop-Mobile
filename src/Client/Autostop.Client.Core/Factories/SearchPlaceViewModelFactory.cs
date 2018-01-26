using Autofac;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.IoC;
using ViewModelKeys = Autostop.Common.Shared.Constants.IoC.ViewModelKeys;

namespace Autostop.Client.Core.Factories
{
    public class SearchPlaceViewModelFactory : ISearchPlaceViewModelFactory
    {
        public IBaseSearchPlaceViewModel GetPickupSearchPlaceViewModel()
        {
            return Locator.ResolveNamed<IBaseSearchPlaceViewModel>(ViewModelKeys.PickupSearch);
        }

        public IBaseSearchPlaceViewModel DestinationSearchPlaceViewModel(ITripLocationViewModel tripLocationViewModel)
        {
            return Locator.ResolveNamed<IBaseSearchPlaceViewModel>(ViewModelKeys.DestinationSearch, new NamedParameter(nameof(tripLocationViewModel), tripLocationViewModel));
        }
    }
}
