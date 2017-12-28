using Autostop.Client.Abstraction.Models;
using Autostop.Client.Core.ViewModels;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Models
{
    public class AddressModel : BaseViewModel, IAddressModel
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

	    public void SetAddress(Address address)
	    {
		    FormattedAddress = address.FormattedAddress;
		    Location = address.Location;
		}
    }
}