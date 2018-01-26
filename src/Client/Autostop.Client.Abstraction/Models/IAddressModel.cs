using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Models
{
    public interface IAddressModel
    {
        string FormattedAddress { get; set; }

        Location Location { get; set; }

		bool Loading { get; set; }

	    void SetAddress(Address address);
    }
}