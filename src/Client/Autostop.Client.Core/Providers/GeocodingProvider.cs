using System.Linq;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Common.Shared.Models;
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

		public async Task<string> ReverseGeocoding(Coordinate coordinate)
		{
			var request = new GeocodingRequest {Address = new LatLng(coordinate.Latitude, coordinate.Longitude)};
			var response = await _geocodingService.GetResponseAsync(request);

		    var address = response.Results.FirstOrDefault();

		    return address.FormattedAddress;
		}
    }
}
