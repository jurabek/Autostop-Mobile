using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap.Base;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap
{
    public class ChooseWorkAddressOnMapViewModel : ChooseAddressOnMapViewModelBase
    {
        public ChooseWorkAddressOnMapViewModel(
            INavigationService navigationService,
            ILocationManager locationManager,
            IGeocodingProvider geocodingProvider) : base(navigationService, locationManager, geocodingProvider)
        {
        }
    }
}
