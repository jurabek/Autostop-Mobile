using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor.Base;

namespace Autostop.Client.Core.ViewModels.Passenger.LocationEditor
{
    public class HomeLocationEditorViewModel : BaseLocationEditorViewModel
    {
		private readonly INavigationService _navigationService;
		private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;
		private readonly ISettingsProvider _settingsProvider;
		private readonly IGeocodingProvider _geocodingProvider;

		public HomeLocationEditorViewModel(
            ISchedulerProvider schedulerProvider,
            INavigationService navigationService,
            IPlacesProvider placesProvider,
            IEmptyAutocompleteResultProvider autocompleteResultProvider,
            ISettingsProvider settingsProvider,
            IGeocodingProvider geocodingProvider) : base(schedulerProvider, placesProvider, geocodingProvider, navigationService)
        {
			_navigationService = navigationService;
			_autocompleteResultProvider = autocompleteResultProvider;
			_settingsProvider = settingsProvider;
			_geocodingProvider = geocodingProvider;

			SelectedAutoCompleteResultModelObservable                
                .Subscribe(async result =>
                {
                    var address = await geocodingProvider.ReverseGeocodingFromPlaceId(result.PlaceId);
                    settingsProvider.SetHomeAddress(address);
                });

            this.Changed(() => SelectedSearchResult)
                .Where(r => r is SetLocationOnMapResultModel)
                .Subscribe(result =>
                {
                    navigationService.NavigateTo<ChooseHomeAddressOnMapViewModel>();
                });
        }

		protected override async Task SetAutoCompleteResultModel(string placeId)
		{
			var address = await _geocodingProvider.ReverseGeocodingFromPlaceId(placeId);
			_settingsProvider.SetHomeAddress(address);
			_navigationService.GoBack();
		}

		protected override ObservableCollection<IAutoCompleteResultModel> GetEmptyAutocompleteResult()
        {
            return new ObservableCollection<IAutoCompleteResultModel>
            {
                _autocompleteResultProvider.GetSetLocationOnMapResultModel()
            };
        }

        public override string PlaceholderText => "Set home address";
    }
}
