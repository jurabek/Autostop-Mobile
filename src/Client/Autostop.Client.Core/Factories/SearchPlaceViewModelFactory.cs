using Autofac;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.Core.IoC;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using JetBrains.Annotations;
using ViewModelKeys = Autostop.Common.Shared.Constants.IoC.ViewModelKeys;

namespace Autostop.Client.Core.Factories
{
    [UsedImplicitly]
    public class SearchPlaceViewModelFactory : ISearchPlaceViewModelFactory
    {
        public IBaseSearchPlaceViewModel GetPickupSearchPlaceViewModel()
        {
            return Locator.ResolveNamed<IBaseSearchPlaceViewModel>(ViewModelKeys.PickupSearch);
        }

        public IBaseSearchPlaceViewModel DestinationSearchPlaceViewModel(IRideViewModel rideViewModel)
        {
            return Locator.ResolveNamed<IBaseSearchPlaceViewModel>(ViewModelKeys.DestinationSearch, new NamedParameter(nameof(rideViewModel), rideViewModel));
        }
    }
}
