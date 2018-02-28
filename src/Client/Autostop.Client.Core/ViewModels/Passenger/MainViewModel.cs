using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class MainViewModel : BaseViewModel, IMainViewModel, IMapViewModel
	{
		private ObservableCollection<DriverLocation> _onlineDrivers = new ObservableCollection<DriverLocation>();
		public ITripLocationViewModel TripLocationViewModel { get; }
		private readonly IGeocodingProvider _geocodingProvider;
		private readonly ILocationManager _locationManager;
		private readonly ISchedulerProvider _schedulerProvider;
		private Location _cameraTarget;
		private List<IDisposable> _subscribers;

		public MainViewModel(
			ISchedulerProvider schedulerProvider,
			IGeocodingProvider geocodingProvider,
			ILocationManager locationManager,
			ITripLocationViewModel tripLocationViewModel)
		{
			_schedulerProvider = schedulerProvider;
			_geocodingProvider = geocodingProvider;
			_locationManager = locationManager;

			TripLocationViewModel = tripLocationViewModel;
			MyLocationChanged = locationManager.LocationChanged;

		}

		public IObservable<Location> MyLocationChanged { get; }

		public IObservable<Location> CameraPositionChanged { get; set; }

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

		private ICommand _goToMyLocation;
		public ICommand GoToMyLocation => _goToMyLocation ?? (_goToMyLocation = new RelayCommand(
			async () => await GetMyLocation()));


		public override async Task Load()
		{
			await base.Load();

			_subscribers = new List<IDisposable>
			{
				TripLocationViewModel.PickupLocationChanged
					.Subscribe(_ => CameraTarget = TripLocationViewModel.PickupAddress.Location),

				CameraStartMoving
					.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(moving =>
					{
						TripLocationViewModel.PickupAddress.FormattedAddress = "Searching...";
						TripLocationViewModel.PickupAddress.Loading = true;
					}),

				VisibleRegionChanged
					.Subscribe(r =>
					{
						OnlineDrivers = new ObservableCollection<DriverLocation>(MockData.AvailableDrivers);
					}),

				CameraPositionChanged
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
			try
			{
				var address = await _geocodingProvider.ReverseGeocodingFromLocation(location);
				if (address != null)
				{
					TripLocationViewModel.PickupAddress.SetAddress(address);
				}
				else
				{
					TripLocationViewModel.PickupAddress.FormattedAddress = "Not found!";
				}
			}
			catch (Exception e)
			{
				Debug.Write(e);
			}
			finally
			{
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