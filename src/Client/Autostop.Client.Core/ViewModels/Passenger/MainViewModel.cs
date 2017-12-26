using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Client.Core.Enums;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using Autostop.Common.Shared.Models;
using Conditions;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class MainViewModel : BaseViewModel, IMainViewModel
	{
		public PickupSearchPlaceViewModel PickupSearchPlaceViewModel { get; }
		public DestinationSearchPlaceViewModel DestinationSearchPlaceViewModel { get; }
		public IRideViewModel RideViewModel { get; }
		private readonly IGeocodingProvider _geocodingProvider;
		private readonly ILocationManager _locationManager;
		private readonly INavigationService _navigationService;
		
		private IDisposable _cameraPositionSubscriber;
		private IDisposable _camerStartMovingSubscriber;
		private IDisposable _myLocationSubscriber;

		private ObservableCollection<DriverLocation> _onlineDrivers;
		private Location _cameraTarget;

		public MainViewModel(
			PickupSearchPlaceViewModel pickupSearchPlaceViewModel,
			DestinationSearchPlaceViewModel destinationSearchPlaceViewModel,
			INavigationService navigationService,
			IGeocodingProvider geocodingProvider,
			ILocationManager locationManager,
			IRideViewModel rideViewModel)
		{
			PickupSearchPlaceViewModel = pickupSearchPlaceViewModel;
			DestinationSearchPlaceViewModel = destinationSearchPlaceViewModel;
			RideViewModel = rideViewModel;
			geocodingProvider.Requires(nameof(geocodingProvider)).IsNotNull();
			locationManager.Requires(nameof(locationManager)).IsNotNull();

			_navigationService = navigationService;
			_geocodingProvider = geocodingProvider;
			_locationManager = locationManager;
			locationManager.StartUpdatingLocation();

			MyLocationObservable = locationManager.LocationChanged;
			NavigateToDestinationSearch = new RelayCommand(NavigateToDistinationSearchViewModel);
			NavigateToPickupSearch = new RelayCommand(NavigateToPickupSearchViewModel);
			GoToMyLocation = new RelayCommand(GoToMyLocationAction);
		}

		private void GoToMyLocationAction()
		{
			CameraTarget = _locationManager.Location;
		}

		public IObservable<Location> MyLocationObservable { get; }

		public IObservable<Location> CameraPositionObservable { [UsedImplicitly] get; set; }

		public IObservable<bool> CameraStartMoving { get; set; }

		public Location CameraTarget
		{
			get => _cameraTarget;
			set
			{
				_cameraTarget = value;
				RaisePropertyChanged();
			}
		}
		
		public ObservableCollection<DriverLocation> OnlineDrivers
		{
			get => _onlineDrivers;
			set => RaiseAndSetIfChanged(ref _onlineDrivers, value);
		}

		public ICommand GoToMyLocation { get; }

		public ICommand NavigateToPickupSearch { get; }

		public ICommand NavigateToDestinationSearch { get; }

		public override Task Load()
		{
			_myLocationSubscriber = MyLocationObservable
				.Subscribe(async location =>
				{
					CameraTarget = _locationManager.Location;
					await CameraLocationChanged(location);
					_myLocationSubscriber.Dispose();
				});

			_camerStartMovingSubscriber = CameraStartMoving
				.Subscribe(moving =>
				{
					RideViewModel.IsPickupAddressLoading = true;
				});

			_cameraPositionSubscriber = CameraPositionObservable
				.Subscribe(async location =>
				{
					await CameraLocationChanged(location);
				});
			
			DestinationSearchPlaceViewModel.SelectedAddress
				.Subscribe(address =>
				{
					RideViewModel.DestinationAddress.SetAddress(address);
					_navigationService.GoBack();
				});

			PickupSearchPlaceViewModel.SelectedAddress
				.Subscribe(address =>
				{
					RideViewModel.PickupAddress.SetAddress(address);
					CameraTarget = address.Location;
					_navigationService.GoBack();
				});

			OnlineDrivers = new ObservableCollection<DriverLocation>(MockData.AvailableDrivers);
			return base.Load();
		}

		private async Task CameraLocationChanged(Location location)
		{
			var address = await _geocodingProvider.ReverseGeocodingFromLocation(location);
			if (address != null)
			{
				RideViewModel.PickupAddress.SetAddress(address);
				RideViewModel.IsPickupAddressLoading = false;
			}
		}
		
		private void NavigateToPickupSearchViewModel()
		{
			_navigationService.NavigateToSearchView(PickupSearchPlaceViewModel);
			PickupSearchPlaceViewModel.SearchText = RideViewModel.PickupAddress.FormattedAddress;
		}

		private void NavigateToDistinationSearchViewModel()
		{
			_navigationService.NavigateToSearchView(DestinationSearchPlaceViewModel);
			DestinationSearchPlaceViewModel.SearchText = RideViewModel.DestinationAddress.FormattedAddress;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				//_locationManager.StopUpdatingLocation();
				_cameraPositionSubscriber.Dispose();
				_myLocationSubscriber.Dispose();
				_camerStartMovingSubscriber.Dispose();
			}
		}
	}
}