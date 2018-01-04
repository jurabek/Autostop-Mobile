using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
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
        private readonly INavigationService _navigationService;
        private readonly IBaseSearchPlaceViewModel _pickupSearchPlaceViewModel;
        private readonly IBaseSearchPlaceViewModel _destinationSearchPlaceViewModel;

        private ObservableCollection<DriverLocation> _onlineDrivers;
        private Location _cameraTarget;
        private List<IDisposable> _subscribers;
        private IDisposable _myLocationObservable;

        public MainViewModel(
            ISearchPlaceViewModelFactory searchPlaceViewModelFactory,
            INavigationService navigationService,
            IGeocodingProvider geocodingProvider,
            ILocationManager locationManager,
            IRideViewModel rideViewModel)
        {
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

            _pickupSearchPlaceViewModel = searchPlaceViewModelFactory.GetPickupSearchPlaceViewModel();
            _destinationSearchPlaceViewModel = searchPlaceViewModelFactory.DestinationSearchPlaceViewModel(RideViewModel);
        }

        private void GoToMyLocationAction()
        {
            CameraTarget = _locationManager.Location;
        }

        public IObservable<Location> MyLocationObservable { get; }

        public IObservable<Location> CameraPositionObservable { get; set; }

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
            set => RaiseAndSetIfChanged(ref _onlineDrivers, value);
        }

        public ICommand GoToMyLocation { get; }

        public ICommand NavigateToPickupSearch { get; }

        public ICommand NavigateToDestinationSearch { get; }

        public override Task Load()
        {
            _myLocationObservable = MyLocationObservable
                .Subscribe(async location =>
                {
                    CameraTarget = _locationManager.Location;
                    await CameraLocationChanged(location);
                    _myLocationObservable.Dispose();
                });

            _subscribers = new List<IDisposable>
            {
                CameraStartMoving
                    .Subscribe(moving =>
                    {
                        RideViewModel.IsPickupAddressLoading = true;
                    }),

                CameraPositionObservable
                    .Subscribe(async location =>
                    {
                        await CameraLocationChanged(location);
                    }),

                _destinationSearchPlaceViewModel.SelectedAddress
                    .Subscribe(address =>
                    {
                        RideViewModel.DestinationAddress.SetAddress(address);
                        _navigationService.GoBack();
                    }),

                _pickupSearchPlaceViewModel.SelectedAddress
					.ObserveOn(SynchronizationContext.Current)
					.Subscribe(address =>
                    {
                        RideViewModel.PickupAddress.SetAddress(address);
                        CameraTarget = address.Location;
                        _navigationService.GoBack();
                    }),
            };

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
            _navigationService.NavigateToSearchView(_pickupSearchPlaceViewModel as PickupSearchPlaceViewModel);
            _pickupSearchPlaceViewModel.SearchText = RideViewModel.PickupAddress.FormattedAddress;
        }

        private void NavigateToDistinationSearchViewModel()
        {
            _navigationService.NavigateToSearchView(_destinationSearchPlaceViewModel as DestinationSearchPlaceViewModel);
            _destinationSearchPlaceViewModel.SearchText = RideViewModel.DestinationAddress.FormattedAddress;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _locationManager.StopUpdatingLocation();
                _subscribers.ForEach(d => d.Dispose());
            }
        }
    }
}