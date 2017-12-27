using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	public sealed class PickupSearchPlaceViewModel : BaseSearchPlaceViewModel
	{
		private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

		public PickupSearchPlaceViewModel(
			INavigationService navigationService,
			IPlacesProvider placesProvider,
			IGeocodingProvider geocodingProvider,
			IEmptyAutocompleteResultProvider autocompleteResultProvider) : base(placesProvider, geocodingProvider, navigationService)
		{
			_autocompleteResultProvider = autocompleteResultProvider;
		}

		protected override ObservableCollection<IAutoCompleteResultModel> GetEmptyAutocompleteResult()
		{
			return new ObservableCollection<IAutoCompleteResultModel>
			{	
				_autocompleteResultProvider.GetHomeResultModel(),
				_autocompleteResultProvider.GetWorkResultModel()
			};
		}

	    public override string PlaceholderText => "Set pickup location";

	}
}