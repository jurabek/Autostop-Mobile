using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.Trip
{
	public class TripLocationViewModel : BaseViewModel, ITripLocationViewModel
	{
		private readonly IBaseLocationEditorViewModel _pickupLocationEditorViewModel;
		private readonly IBaseLocationEditorViewModel _destinationLocationEditorViewModel;
		private readonly INavigationService _navigationService;
		public event EventHandler<EventArgs> PickupLocationChanged;

		public TripLocationViewModel(
			ISchedulerProvider schedulerProvider,
			INavigationService navigationService,
			ISearchPlaceViewModelFactory searchPlaceViewModelFactory)
		{
			_pickupLocationEditorViewModel = searchPlaceViewModelFactory.GetPickupLocationEditorViewModel();
			_destinationLocationEditorViewModel = searchPlaceViewModelFactory.GetDestinationLocationEditorViewModel(this);
			_navigationService = navigationService;
			
			_pickupLocationEditorViewModel.SelectedAddress
				.ObserveOn(schedulerProvider.SynchronizationContextScheduler)
				.Subscribe(address =>
				{
					PickupAddress.SetAddress(address);
					PickupLocationChanged?.Invoke(this, EventArgs.Empty);
					_navigationService.GoBack();
				});

			_destinationLocationEditorViewModel.SelectedAddress
				.ObserveOn(schedulerProvider.SynchronizationContextScheduler)
				.Subscribe(address =>
				{
					DestinationAddress.SetAddress(address);
					CanRequest = true;
					_navigationService.GoBack();
				});
		}

		public IAddressModel PickupAddress { get; } = new AddressModel();

		public IAddressModel DestinationAddress { get; } = new AddressModel();
	
		public ICommand NavigateToPickupSearch => new RelayCommand(
			() =>
			{
				_navigationService.NavigateToSearchView(_pickupLocationEditorViewModel as PickupLocationEditorViewModel);
				_pickupLocationEditorViewModel.SearchText = PickupAddress.FormattedAddress;
			});

		public ICommand NavigateToWhereTo => new RelayCommand(
			() =>
			{
				_navigationService.NavigateToSearchView(_destinationLocationEditorViewModel as DestinationLocationEditorViewModel);
			});

		private bool _canRequest;

		public bool CanRequest
		{
			get => _canRequest;
			set => RaiseAndSetIfChanged(ref _canRequest, value);
		}
	}
}
