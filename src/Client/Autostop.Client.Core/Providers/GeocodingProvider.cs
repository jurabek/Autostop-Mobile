using System.Threading.Tasks;
using Autostop.Client.Abstraction.Providers;
using Conditions;
using Google.Maps;
using Google.Maps.Geocoding;

namespace Autostop.Client.Core.Providers
{
	public class GeocodingProvider : IGeocodingProvider
	{
		private readonly IGeocodingService _geocodingService;
		public GeocodingProvider(IGeocodingService geocodingService)
		{
			geocodingService.Requires(nameof(geocodingService));

			_geocodingService = geocodingService;
		}

		public Task<GeocodeResponse> ReverseGeocoding(double lat, double lng)
		{
			var request = new GeocodingRequest {Address = new LatLng(lat, lng)};
			return _geocodingService.GetResponseAsync(request);
		}
	}
}
