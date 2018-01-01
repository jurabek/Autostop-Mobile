using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Providers
{
    public interface IVisibleRegionProvider
    {
	    VisibleRegion GetVisibleRegion(Location northEast, Location southWest);
    }
}
