using System.Linq;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Providers;
using Autostop.Common.Shared.Models;
using Conditions;
using Google.Maps;
using Google.Maps.Geocoding;
using Location = Autostop.Common.Shared.Models.Location;

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

		public async Task<Address> ReverseGeocoding(Location location)
		{
			var request = new GeocodingRequest {Address = new LatLng(location.Latitude, location.Longitude)};
			var response = await _geocodingService.GetResponseAsync(request);

		    var address = response.Results.FirstOrDefault();

			return new Address
			{
				FormattedAddress = address.FormattedAddress,
				Location = location
			};
		}
    }
}
