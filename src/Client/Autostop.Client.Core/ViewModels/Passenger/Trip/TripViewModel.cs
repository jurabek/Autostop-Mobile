using System.Windows.Input;

namespace Autostop.Client.Core.ViewModels.Passenger.Trip
{
	public class TripViewModel : BaseViewModel
	{
		public TripPriceViewModel TripPriceViewModel { get; set; }

		public ICommand RequestCommand { get; set; }

		public TripStatus Status { get; set; }
	}
}
