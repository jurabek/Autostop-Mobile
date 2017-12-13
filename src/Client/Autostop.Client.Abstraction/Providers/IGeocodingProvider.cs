using System.Threading.Tasks;
using Google.Maps.Geocoding;

namespace Autostop.Client.Abstraction.Providers
{
	public interface IGeocodingProvider
	{
		Task<GeocodeResponse> ReverseGeocoding(double lat, double lng);
	}
}