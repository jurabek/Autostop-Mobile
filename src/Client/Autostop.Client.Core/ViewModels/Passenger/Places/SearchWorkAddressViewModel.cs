using System;
using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	public class SearchWorkAddressViewModel : BaseSearchPlaceViewModel
	{
		private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

		public SearchWorkAddressViewModel(
			INavigationService navigationService,
			IPlacesProvider placesProvider,
			IEmptyAutocompleteResultProvider autocompleteResultProvider,
			ISettingsProvider settingsProvider,
			IGeocodingProvider geocodingProvider) : base(placesProvider, navigationService)
		{
			_autocompleteResultProvider = autocompleteResultProvider;
			PlaceholderText = "Set work address";

			this.ObservablePropertyChanged(() => SelectedSearchResult)
				.Subscribe(async result =>
				{
					if (result is AutoCompleteResultModel)
					{
						var address = await geocodingProvider.ReverseGeocodingFromPlaceId(result.PlaceId);
						settingsProvider.WorkAddress = new Address
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
	}
}
