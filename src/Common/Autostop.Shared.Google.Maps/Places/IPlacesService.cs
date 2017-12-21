using System.Threading.Tasks;
using Google.Maps.Places.Autocomplete;

namespace Google.Maps.Places
{
    public interface IPlacesService
    {
        AutocompleteResponse GetAutocompleteResponse(AutocompleteRequest request);
        Task<AutocompleteResponse> GetAutocompleteResponseAsync(AutocompleteRequest request);
        PlacesResponse GetResponse<TRequest>(TRequest request) where TRequest : PlacesRequest;
        Task<PlacesResponse> GetResponseAsync<TRequest>(TRequest request) where TRequest : PlacesRequest;
    }
}