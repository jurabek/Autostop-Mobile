using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Extensions;
using System;
using Autostop.Client.Core.Models;
using Autostop.Common.Shared.Models;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
    [UsedImplicitly]
    public sealed class SearchHomeAddressViewModel : BaseSearchPlaceViewModel
    {
	    private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

	    public SearchHomeAddressViewModel(
			INavigationService navigationService,
			IPlacesProvider placesProvider,
		    IEmptyAutocompleteResultProvider autocompleteResultProvider,
			ISettingsProvider settingsProvider,
			IGeocodingProvider geocodingProvider) : base(placesProvider, geocodingProvider, navigationService)
	    {
		    _autocompleteResultProvider = autocompleteResultProvider;

		    this.ObservablePropertyChanged(() => SelectedSearchResult)
				.Subscribe(async result =>
				{
					if (result is AutoCompleteResultModel)
					{
						var address = await geocodingProvider.ReverseGeocodingFromPlaceId(result.PlaceId);
						settingsProvider.HomeAddress = new Address
						{
							FormattedAddress = address.FormattedAddress,
							Location = address.Location
						};
						GoBackCallback?.Invoke();
					}
				});
	    }

		public Action GoBackCallback { get; set; }

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
