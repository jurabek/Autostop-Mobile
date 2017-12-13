using Autostop.Client.Abstraction.Adapters;
using Google.Maps;
using JetBrains.Annotations;
using Location = Autostop.Common.Shared.Models.Location;

namespace Autostop.Client.Core.Adapters
{
	[UsedImplicitly]
	public class LocationAdapter : ILocationAdapter
	{
		public Location GetLocationFromCoordinate(double latitude, double longitude)
		{
			return new Location(latitude, longitude);
		}

		public LatLng GetLocation(Location location)
		{
			return new LatLng(location.Coordinate.Latitude, location.Coordinate.Longitude);
		}

		public Location GetLocation(LatLng location)
		{
			return new Location(location.Latitude, location.Longitude);
		}
	}
}
