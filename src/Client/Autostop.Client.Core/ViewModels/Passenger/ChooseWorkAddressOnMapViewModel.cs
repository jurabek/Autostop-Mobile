using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    [UsedImplicitly]
    public class ChooseWorkAddressOnMapViewModel : BaseChooseAddressOnMapViewModel
    {
        public ChooseWorkAddressOnMapViewModel(
            ILocationManager locationManager,
            IGeocodingProvider geocodingProvider) : base(locationManager, geocodingProvider)
        {
        }
    }
}
