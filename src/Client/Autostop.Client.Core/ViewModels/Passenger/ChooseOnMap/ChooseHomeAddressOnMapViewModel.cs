using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap
{
    [UsedImplicitly]
    public class ChooseHomeAddressOnMapViewModel : ChooseAddressOnMapViewModelBase
    {
        public ChooseHomeAddressOnMapViewModel(
            INavigationService navigationService,
            ILocationManager locationManager,
            IGeocodingProvider geocodingProvider) : base(navigationService, locationManager, geocodingProvider)
        {
        }
    }
}
