using Autofac;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.Core.IoC;

namespace Autostop.Client.Core.Factories
{
    public class ChooseOnMapViewModelFactory : IChooseOnMapViewModelFactory
    {
        public ISearchableViewModel GetChooseDestinationOnMapViewModel(IRideViewModel rideViewModel)
        {
            return Locator.Container.
                ResolveNamed<ISearchableViewModel>(
                    Common.Shared.Constants.IoC.ViewModelKeys.ChooseDestinationOnMap,
                    new NamedParameter(nameof(rideViewModel), rideViewModel));
        }
    }
}
