using Android.Gms.Maps.Model;
using Autostop.Client.Android.Platform.Android.Abstraction;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Android.Adapters
{
	public class MarkerAdapter : IMarkerAdapter
	{
		public MarkerOptions GetMarkerOptions(DriverLocation driverLocation)
		{
			var markerOptions = new MarkerOptions()
				.SetPosition(new LatLng(driverLocation.CurrentLocation.Latitude, driverLocation.CurrentLocation.Longitude))
				.Anchor((float) 0.5, (float) 0.5)
				.SetRotation((float) driverLocation.Bearing)
				.Flat(true);
			return markerOptions;
		}
	}
}