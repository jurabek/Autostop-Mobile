using System.Windows.Input;

namespace Autostop.Client.Core.ViewModels.Passenger.Trip
{
	public class TripViewModel : BaseViewModel
	{
		public TripLocationViewModel TripLocationViewModel { get; set; }

		public TripPriceViewModel TripPriceViewModel { get; set; }

		public ICommand RequestCommand { get; set; }

		public TripStatus Status { get; set; }
	}
}
