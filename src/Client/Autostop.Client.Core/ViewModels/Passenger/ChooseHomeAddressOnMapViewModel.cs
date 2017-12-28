using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    [UsedImplicitly]
    public class ChooseHomeAddressOnMapViewModel : BaseChooseAddressOnMapViewModel
    {
        public ChooseHomeAddressOnMapViewModel(
            ILocationManager locationManager,
            IGeocodingProvider geocodingProvider) : base(locationManager, geocodingProvider)
        {
        }
    }
}
