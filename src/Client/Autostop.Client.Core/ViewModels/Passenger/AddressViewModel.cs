using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public class AddressViewModel : BaseViewModel, IAddressViewModel
    {
        private string _formattedAddress;

        private Location _location;

        public string FormattedAddress
        {
            get => _formattedAddress;
            set => RaiseAndSetIfChanged(ref _formattedAddress, value);
        }

        public Location Location
        {
            get => _location;
            set => RaiseAndSetIfChanged(ref _location, value);
        }
    }
}