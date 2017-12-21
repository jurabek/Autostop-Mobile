using System.Threading.Tasks;

namespace Google.Maps.Geocoding
{
    public interface IGeocodingService
    {
        GeocodeResponse GetResponse(GeocodingRequest request);
        Task<GeocodeResponse> GetResponseAsync(GeocodingRequest request);
    }
}