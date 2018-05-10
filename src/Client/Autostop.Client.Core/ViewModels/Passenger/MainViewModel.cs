using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class MainViewModel : BaseViewModel, IMainViewModel, IMapViewModel
	{
		private readonly INavigationService _navigationService;

		private ObservableCollection<DriverLocation> _onlineDrivers = new ObservableCollection<DriverLocation>();
		private readonly IGeocodingProvider _geocodingProvider;
		private readonly ILocationManager _locationManager;
		private readonly ISchedulerProvider _schedulerProvider;
		private Location _cameraTarget;
		private List<IDisposable> _subscribers;
		private ICommand _navigateWhereTo;
		private ICommand _navigatePickupSearch;
		private ICommand _goToMyLocation;
		private bool _canRequest;

		public MainViewModel(
			ISchedulerProvider schedulerProvider,
			IGeocodingProvider geocodingProvider,
			ILocationManager locationManager,
			INavigationService navigationService,
			ISearchPlaceViewModelFactory searchPlaceViewModelFactory)
		{
			_schedulerProvider = schedulerProvider;
			_geocodingProvider = geocodingProvider;
			_locationManager = locationManager;

			MyLocationChanged = locationManager.LocationChanged;


			PickupLocationEditorViewModel = searchPlaceViewModelFactory.GetPickupLocationEditorViewModel();
			DestinationLocationEditorViewModel = searchPlaceViewModelFactory.GetDestinationLocationEditorViewModel();
			_navigationService = navigationService;
		}

		public IBaseLocationEditorViewModel PickupLocationEditorViewModel { get; }

		public IBaseLocationEditorViewModel DestinationLocationEditorViewModel { get; }

		public IObservable<Location> MyLocationChanged { get; }

		public IObservable<Location> CameraPositionChanged { get; set; }

		public IObservable<bool> CameraStartMoving { get; set; }

		public IObservable<VisibleRegion> VisibleRegionChanged { get; set; }

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

		public ICommand GoToMyLocation => _goToMyLocation ?? (_goToMyLocation = new RelayCommand(
			async () => await GetMyLocation()));

		public ICommand NavigateToPickupSearch => _navigatePickupSearch ?? (_navigatePickupSearch = new RelayCommand(() =>
		{
			_navigationService.NavigateToSearchView(PickupLocationEditorViewModel as PickupLocationEditorViewModel);
			PickupLocationEditorViewModel.SearchText = PickupLocationEditorViewModel.SelectedAddress.FormattedAddress;
		}));

		public ICommand NavigateToWhereTo => _navigateWhereTo ?? (_navigateWhereTo = new RelayCommand(() =>
		{
			_navigationService.NavigateToSearchView(DestinationLocationEditorViewModel as DestinationLocationEditorViewModel);
		}));

		public bool CanRequest
		{
			get => _canRequest;
			set => RaiseAndSetIfChanged(ref _canRequest, value);
		}

		public override async Task Load()
		{
			_subscribers = new List<IDisposable>
			{
				CameraStartMoving.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(moving =>
					{
						PickupLocationEditorViewModel.SelectedAddress.FormattedAddress = "Searching...";
						PickupLocationEditorViewModel.IsSearching = true;
					}),

				VisibleRegionChanged.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(r =>
					{
						OnlineDrivers = new ObservableCollection<DriverLocation>(MockData.AvailableDrivers);
					}),

				CameraPositionChanged.SelectMany(l => _geocodingProvider.ReverseGeocodingFromLocation(l))
					.ObserveOn(_schedulerProvider.SynchronizationContextScheduler)
					.Subscribe(address =>
					{
						PickupLocationEditorViewModel.SelectedAddress = address;
						PickupLocationEditorViewModel.IsSearching = false;
					}, ex =>
					{
						Debug.Write(ex);
						PickupLocationEditorViewModel.IsSearching = false;
					})
			};

			await base.Load();
		}

		public async Task GetMyLocation()
		{
			var lastLocation = await _locationManager.RequestSingleLocationUpdate();
			CameraTarget = lastLocation;
			var address = await _geocodingProvider.ReverseGeocodingFromLocation(CameraTarget);
			PickupLocationEditorViewModel.SelectedAddress = address;
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