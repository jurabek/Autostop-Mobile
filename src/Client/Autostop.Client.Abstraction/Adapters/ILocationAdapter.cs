using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Adapters
{
    public interface ILocationAdapter
    {
	    Location GetLocationFromCoordinate(double latitude, double longitude);
    }
}
