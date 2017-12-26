using System.Threading.Tasks;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Providers
{
    public interface IGeocodingProvider
    {
        Task<Address> ReverseGeocodingFromLocation(Location location);

	    Task<Address> ReverseGeocodingFromPlaceId(string placeId);
    }
}