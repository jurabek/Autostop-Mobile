using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using Autostop.Common.Shared.Models;
using Conditions;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class MainViewModel : BaseViewModel, IMainViewModel, IMapViewModel
	{
		public IRideViewModel RideViewModel { get; }
		private readonly IGeocodingProvider _geocodingProvider;
		private readonly ILocationManager _locationManager;
		private readonly ISchedulerProvider _schedulerProvider;
		private readonly INavigationService _navigationService;
		private readonly IBaseSearchPlaceViewModel _pickupSearchPlaceViewModel;
		private readonly IBaseSearchPlaceViewModel _destinationSearchPlaceViewModel;
		private ObservableCollection<DriverLocation> _onlineDrivers = new ObservableCollection<DriverLocation>();
		private Location _cameraTarget;
		private List<IDisposable> _subscribers;

		public MainViewModel(
			ISchedulerProvider schedulerProvider,
			ISearchPlaceViewModelFactory searchPlaceViewModelFactory,
			INavigationService navigationService,
			IGeocodingProvider geocodingProvider,
			ILocationManager locationManager,
			IRideViewModel rideViewModel)
		{
			schedulerProvider.Requires(nameof(schedulerProvider)).IsNotNull();
			searchPlaceViewModelFactory.Requires(nameof(searchPlaceViewModelFactory)).IsNotNull();
			navigationService.Requires(nameof(navigationService)).IsNotNull();
			geocodingProvider.Requires(nameof(geocodingProvider)).IsNotNull();
			locationManager.Requires(nameof(locationManager)).IsNotNull();
			rideViewModel.Requires(nameof(rideViewModel)).IsNotNull();

			_schedulerProvider = schedulerProvider;
			_navigationService = navigationService;
			_geocodingProvider = geocodingProvider;
			_locationManager = locationManager;
			RideViewModel = rideViewModel;

			MyLocationObservable = locationManager.LocationChanged;
			_pickupSearchPlaceViewModel = searchPlaceViewModelFactory.GetPickupSearchPlaceViewModel();
			_destinationSearchPlaceViewModel = searchPlaceViewModelFactory.DestinationSearchPlaceViewModel(RideViewModel);
		}

		public IObservable<Location> MyLocationObservable { get; }

		public IObservable<Location> CameraPositionObservable { get; set; }

		public IObservable<bool> CameraStartMoving { get; set; }

		public IObservable<VisibleRegion> VisibleRegionChanged { [UsedImplicitly] get; set; }

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
			private set
			{
				_onlineDrivers = value;
				RaisePropertyChanged();
			}
		}

		public ICommand GoToMyLocation => new RelayCommand(
			async () => await GetMyLocation());

		public ICommand NavigateToPickupSearch => new RelayCommand(
			() =>
			{
				_navigationService.NavigateToSearchView(_pickupSearchPlaceViewModel as PickupSearchPlaceViewModel);
				_pickupSearchPlaceViewModel.SearchText = RideViewModel.PickupAddress.FormattedAddress;
			});

		public ICommand NavigateToWhereTo => new RelayCommand(
			() =>
			{
				_navigationService.NavigateToSearchView(_destinationSearchPlaceViewModel as DestinationSearchPlaceViewModel);
				_destinationSearchPlaceViewModel.SearchText = string.Empty;
			});

		public override async Task Load()
		{
			await base.Load();

			_subscribers = new List<IDisposable>
			{
				CameraStartMoving
					.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(moving =>
					{
						RideViewModel.IsPickupAddressLoading = true;
					}),

				VisibleRegionChanged
					.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(r =>
					{
						OnlineDrivers = new ObservableCollection<DriverLocation>(MockData.AvailableDrivers);
					}),

				CameraPositionObservable
					.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(async location =>
					{
						await CameraLocationChanged(location);
					}),

				_destinationSearchPlaceViewModel.SelectedAddress
					.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(address =>
					{
						RideViewModel.DestinationAddress.SetAddress(address);
						_navigationService.GoBack();
					}),

				_pickupSearchPlaceViewModel.SelectedAddress
					.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(address =>
					{
						RideViewModel.PickupAddress.SetAddress(address);
						CameraTarget = address.Location;
						_navigationService.GoBack();
					})
			};
		}

		public async Task GetMyLocation()
		{
			var lastLocation = await _locationManager.RequestSingleLocationUpdate();
			CameraTarget = lastLocation;
			await CameraLocationChanged(lastLocation);
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

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				_locationManager.StopLocationUpdates();
				_subscribers.ForEach(d => d.Dispose());
			}
		}
	}
}