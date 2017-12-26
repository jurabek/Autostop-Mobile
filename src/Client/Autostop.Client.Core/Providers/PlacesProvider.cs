using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels;
using Autostop.Client.Core.ViewModels.Passenger;
using Conditions;
using Google.Maps;
using Google.Maps.Places;
using Google.Maps.Places.Autocomplete;

namespace Autostop.Client.Core.Providers
{
    public class PlacesProvider : IPlacesProvider
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

        public async Task<ObservableCollection<IAutoCompleteResultModel>> GetAutoCompleteResponse(string input)
        {
	        if (input == "AAA")
		        throw new Exception("AAA");

            var currentLocation = _locationManager.Location;
            try
            {
                var request = new AutocompleteRequest
                {
                    Input = input,
                    Location = new LatLng(currentLocation.Latitude, currentLocation.Longitude),
                    Radius = 50000,
                    Language = "en|ru"
                };

                var result = await _placesService.GetAutocompleteResponseAsync(request);

                if (result?.Status != ServiceResponseStatus.Ok)
                    return null;

                var addresses = new ObservableCollection<IAutoCompleteResultModel>(result.Predictions.Select(p =>
                    new AutoCompleteResultModel
                    {
                        PrimaryText = p.StructuredFormatting?.MainText,
                        SecondaryText = p.StructuredFormatting?.SecondaryText,
                        PlaceId = p.PlaceId,
                        Icon = "marker.png"
                    }));

                return addresses;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}