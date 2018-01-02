using Autostop.Common.Shared.Helpers;

namespace Autostop.Common.Shared.Models
{
	public class DriverLocation
	{
		public DriverLocation(double prevLocationLat, double prevLocationLong, double currentLocationLat, double currentLocationLong)
		{
			PreviusLocation = new Location(prevLocationLat, prevLocationLong);
			CurrentLocation = new Location(currentLocationLat, currentLocationLong);
		}

		public Location PreviusLocation { get; set; }

		public Location CurrentLocation { get; set; }

		public double Bearing => GetBearing();

		private double GetBearing()
		{
			//var lat1 = PreviusLocation.Latitude.ToRadians();
			//var lon1 = PreviusLocation.Longitude.ToRadians();

			//var lat2 = CurrentLocation.Latitude.ToRadians();
			//var lon2 = CurrentLocation.Longitude.ToRadians();

			//var dLon = lon2 - lon1;

			//var y = Sin(dLon) * Cos(lat2);
			//var x = Cos(lat1) * Sin(lat2) - Sin(lat1) * Sin(lat2) * Cos(dLon);

			//var radiansBearing = Atan2(y, x);
			
			//return (radiansBearing.ToDegrees() + 360) % 360;

			return PreviusLocation.BearingTo(CurrentLocation);
		}
	}


}
