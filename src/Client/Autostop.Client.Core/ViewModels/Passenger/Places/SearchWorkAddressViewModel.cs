using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Autostop.Common.Shared.Models;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	[UsedImplicitly]
	public sealed class SearchWorkAddressViewModel : BaseSearchPlaceViewModel
	{
		private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

		public SearchWorkAddressViewModel(
			IScheduler scheduler,
			INavigationService navigationService,
			IPlacesProvider placesProvider,
			IEmptyAutocompleteResultProvider autocompleteResultProvider,
			ISettingsProvider settingsProvider,
			IGeocodingProvider geocodingProvider) : base(scheduler, placesProvider, geocodingProvider, navigationService)
		{
			_autocompleteResultProvider = autocompleteResultProvider;

			this.ObservablePropertyChanged(() => SelectedSearchResult)
				.Where(sr => sr is AutoCompleteResultModel)
				.SubscribeOn(scheduler)
				.Subscribe(async result =>
				{
					var address = await geocodingProvider.ReverseGeocodingFromPlaceId(result.PlaceId);
					settingsProvider.WorkAddress = address;
					GoBackCallback?.Invoke();
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

		public override string PlaceholderText => "Set work address";
	}
}
