using Autofac;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.Core.IoC;
using JetBrains.Annotations;
using IoCConstants = Autostop.Common.Shared.Constants.IoC;

namespace Autostop.Client.Core.Factories
{
    [UsedImplicitly]
    public class SearchPlaceViewModelFactory : ISearchPlaceViewModelFactory
    {
        public IBaseSearchPlaceViewModel GetPickupSearchPlaceViewModel()
        {
            return Locator.Container.ResolveNamed<IBaseSearchPlaceViewModel>(IoCConstants.ViewModelKeys.PickupSearch);
        }

        public IBaseSearchPlaceViewModel DestinationSearchPlaceViewModel(IRideViewModel rideViewModel)
        {
            return Locator.Container.
                ResolveNamed<IBaseSearchPlaceViewModel>(
                    IoCConstants.ViewModelKeys.DestinationSearch, new NamedParameter(nameof(rideViewModel), rideViewModel));
        }
    }
}
