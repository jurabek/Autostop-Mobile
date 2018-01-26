using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Common.Shared.Models;
using Conditions;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class MainViewModel : BaseViewModel, IMainViewModel, IMapViewModel
	{
		public ITripLocationViewModel TripLocationViewModel { get; }
		private readonly IGeocodingProvider _geocodingProvider;
		private readonly ILocationManager _locationManager;
		private readonly ISchedulerProvider _schedulerProvider;
		private ObservableCollection<DriverLocation> _onlineDrivers = new ObservableCollection<DriverLocation>();
		private Location _cameraTarget;
		private List<IDisposable> _subscribers;

		public MainViewModel(
			ISchedulerProvider schedulerProvider,
			IGeocodingProvider geocodingProvider,
			ILocationManager locationManager,
			ITripLocationViewModel tripLocationViewModel)
		{
			schedulerProvider.Requires(nameof(schedulerProvider)).IsNotNull();
			geocodingProvider.Requires(nameof(geocodingProvider)).IsNotNull();
			locationManager.Requires(nameof(locationManager)).IsNotNull();
			tripLocationViewModel.Requires(nameof(tripLocationViewModel)).IsNotNull();

			_schedulerProvider = schedulerProvider;
			_geocodingProvider = geocodingProvider;
			_locationManager = locationManager;

			TripLocationViewModel = tripLocationViewModel;
			MyLocationObservable = locationManager.LocationChanged;
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


		public override async Task Load()
		{
			await base.Load();

			_subscribers = new List<IDisposable>
			{
				CameraStartMoving
					.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(moving =>
					{
						TripLocationViewModel.PickupAddress.Loading = true;
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
				TripLocationViewModel.PickupAddress.SetAddress(address);
				TripLocationViewModel.PickupAddress.Loading = false;
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