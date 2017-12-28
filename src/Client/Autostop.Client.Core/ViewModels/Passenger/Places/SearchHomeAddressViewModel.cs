using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Extensions;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Autostop.Client.Core.Models;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
    [UsedImplicitly]
    public sealed class SearchHomeAddressViewModel : BaseSearchPlaceViewModel
    {
	    private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

	    public SearchHomeAddressViewModel(
			ISchedulerProvider schedulerProvider,
			INavigationService navigationService,
			IPlacesProvider placesProvider,
		    IEmptyAutocompleteResultProvider autocompleteResultProvider,
			ISettingsProvider settingsProvider,
			IGeocodingProvider geocodingProvider) : base(schedulerProvider, placesProvider, geocodingProvider, navigationService)
	    {
		    _autocompleteResultProvider = autocompleteResultProvider;

		    this.ObservablePropertyChanged(() => SelectedSearchResult)
				.Subscribe(async result =>
				{
					if (result is AutoCompleteResultModel)
					{
						var address = await geocodingProvider.ReverseGeocodingFromPlaceId(result.PlaceId);
						settingsProvider.HomeAddress = address;
						GoBackCallback?.Invoke();
					}
				});

	        this.ObservablePropertyChanged(() => SelectedSearchResult)
                .Where(r => r is SetLocationOnMapResultModel)
	            .Subscribe(result =>
	            {
                    navigationService.NavigateTo<ChooseHomeAddressOnMapViewModel>();
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
