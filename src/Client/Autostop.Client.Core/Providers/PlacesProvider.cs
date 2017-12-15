using System.Threading.Tasks;
using Autostop.Client.Abstraction.Managers;
using Conditions;
using Google.Maps;
using Google.Maps.Places;

namespace Autostop.Client.Core.Providers
{
    public class PlacesProvider
    {
	    private readonly ILocationManager _locationManager;
	    private readonly IPlacesService _placesService;

	    public PlacesProvider(
			ILocationManager locationManager,
			IPlacesService placesService)
	    {
		    locationManager.Requires(nameof(locationManager));
		    placesService.Requires(nameof(placesService));

			_locationManager = locationManager;
		    _placesService = placesService;
	    }

	    public Task<AutocompleteResponse> GetAutoCompleteResponse(string input, int offset, int radius, string language)
	    {
		    var currentLocation = _locationManager.CurrentLocation;

			var request = new AutocompleteRequest
		    {
			    Input = input,
			    Offset = offset,
			    Radius = radius,
				Location = new LatLng(currentLocation.Latitude, currentLocation.Longitude),
				Language = language
		    };

		    return _placesService.GetAutocompleteResponseAsync(request);
	    }
    }
}
