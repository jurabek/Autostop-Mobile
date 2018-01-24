using System.Windows.Input;
using Autostop.Client.Abstraction.Models;

namespace Autostop.Client.Abstraction.ViewModels
{
	public interface IRideViewModel : IObservableViewModel
	{
		IAddressModel PickupAddress { get; }
		IAddressModel DestinationAddress { get; }
		ICommand SetPickupLocation { get; }
		bool IsPickupAddressLoading { get; set; }
		bool HasPickupLocation { get; set; }
		bool HasDestinationAddress { get; set; }
	}
}