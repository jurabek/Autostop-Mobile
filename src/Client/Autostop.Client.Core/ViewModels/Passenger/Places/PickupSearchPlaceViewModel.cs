using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	public sealed class PickupSearchPlaceViewModel : BaseSearchPlaceViewModel
	{
		private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

		public PickupSearchPlaceViewModel(
			IScheduler scheduler,
			INavigationService navigationService,
			IPlacesProvider placesProvider,
			IGeocodingProvider geocodingProvider,
			IEmptyAutocompleteResultProvider autocompleteResultProvider) : base(scheduler, placesProvider, geocodingProvider, navigationService)
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