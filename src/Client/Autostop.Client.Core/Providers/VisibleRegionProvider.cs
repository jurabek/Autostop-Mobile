using Autostop.Client.Abstraction.Providers;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Providers
{
    public class VisibleRegionProvider : IVisibleRegionProvider
    {
	    public VisibleRegion GetVisibleRegion(Location northEast, Location southWest)
	    {
		    var northWest = new Location(northEast.Latitude, southWest.Longitude);
		    var southEast = new Location(southWest.Latitude, northEast.Longitude);

		    return new VisibleRegion
		    {
			    NorthEast = northEast,
			    SouthWest = southWest,
			    NorthWest = northWest,
			    SouthEast = southEast
		    };
	    }
    }
}
