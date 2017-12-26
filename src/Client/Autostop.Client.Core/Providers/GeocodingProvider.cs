using System;
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

        public async Task<Address> ReverseGeocodingFromLocation(Location location)
        {
            if (!location.IsValid() || Math.Abs(location.Latitude) <= 0 && Math.Abs(location.Longitude) <= 0)
                return null;

            var request = new GeocodingRequest {Address = new LatLng(location.Latitude, location.Longitude)};
            var response = await _geocodingService.GetResponseAsync(request);

            if (response.Status != ServiceResponseStatus.Ok)
                return null;

            var address = response.Results.FirstOrDefault();
            if (address == null)
                return null;

            return new Address
            {
                FormattedAddress = address.FormattedAddress,
                Location = location
            };
        }

	    public async Task<Address> ReverseGeocodingFromPlaceId(string placeId)
	    {
		    if (string.IsNullOrEmpty(placeId))
			    return null;


		    var request = new GeocodingRequest { Address = new PlaceIdLocation(placeId) };
		    var response = await _geocodingService.GetResponseAsync(request);

		    if (response.Status != ServiceResponseStatus.Ok)
			    return null;

		    var address = response.Results.FirstOrDefault();
		    if (address == null)
			    return null;

		    var location = address.Geometry.Location;

			return new Address
		    {
			    FormattedAddress = address.FormattedAddress,
			    Location = new Location(location.Latitude, location.Longitude) 
		    };
	    }
	}
}