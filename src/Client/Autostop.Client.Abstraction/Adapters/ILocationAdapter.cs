using Google.Maps;
using Location = Autostop.Common.Shared.Models.Location;

namespace Autostop.Client.Abstraction.Adapters
{
    public interface ILocationAdapter
    {
	    Location GetLocationFromCoordinate(double latitude, double longitude);

	    LatLng GetLocation(Location location);

	    Location GetLocation(LatLng location);
    }
}
