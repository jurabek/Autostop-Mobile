using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.ViewModels.Passenger
{
    public interface IAddressViewModel
    {
        string FormattedAddress { get; set; }

		Location Location { get; set; }
    }
}