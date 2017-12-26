using System.Windows.Input;
using Autostop.Client.Abstraction.ViewModels.Passenger;

namespace Autostop.Client.Abstraction.ViewModels
{
	public interface IRideViewModel : IObservableViewModel
	{
		IAddressViewModel PickupAddress { get; }
		IAddressViewModel DestinationAddress { get; }
		ICommand SetPickupLocation { get; }
		bool IsPickupAddressLoading { get; set; }
		bool HasPickupLocation { get; set; }
	}
}