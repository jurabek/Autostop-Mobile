using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Models;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.Places
{
	[UsedImplicitly]
	public sealed class PickupSearchPlaceViewModel : BaseSearchPlaceViewModel
	{
		private readonly INavigationService _navigationService;
		private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

		public PickupSearchPlaceViewModel(
			ISchedulerProvider schedulerProviderer,
			INavigationService navigationService,
			IPlacesProvider placesProvider,
			IGeocodingProvider geocodingProvider,
			IEmptyAutocompleteResultProvider autocompleteResultProvider) : base(schedulerProviderer, placesProvider, geocodingProvider, navigationService)
		{
			_navigationService = navigationService;
			_autocompleteResultProvider = autocompleteResultProvider;

			SelectedEmptyAutocompleteResultModelObservable()
				.Where(r => r.Address == null)
				.Subscribe(SelectedEmptyAutocompleteResultModel);
		}
		

		private void SelectedEmptyAutocompleteResultModel(IAutoCompleteResultModel selectedResult)
		{
			switch (selectedResult)
			{
				case HomeResultModel _:
					_navigationService.NavigateToSearchView<SearchHomeAddressViewModel>(vm => vm.GoBackCallback = GoBackCallback);
					break;
				case WorkResultModel _:
					_navigationService.NavigateToSearchView<SearchWorkAddressViewModel>(vm => vm.GoBackCallback = GoBackCallback);
					break;
			}
		}

		private void GoBackCallback()
		{
			_navigationService.GoBack();
			SearchResults = GetEmptyAutocompleteResult();
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