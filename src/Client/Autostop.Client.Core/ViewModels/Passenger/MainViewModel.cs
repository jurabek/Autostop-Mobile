using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Client.Core.Enums;
using Autostop.Common.Shared.Models;
using Conditions;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        private readonly IGeocodingProvider _geocodingProvider;
        private readonly ILocationManager _locationManager;
        private AddressMode _addressMode;
        private IDisposable _cameraPositionSubscriber;
        private bool _cameraUpdated;
        private IDisposable _camerStartMovingSubscriber;
        private bool _isDestinationAddressLoading;
        private bool _isPickupAddressLoading;
        private Location _myLocation;
        private IDisposable _myLocationSubscriber;

        public MainViewModel(
            ILocationManager locationManager,
            IGeocodingProvider geocodingProvider)
        {
            locationManager.Requires(nameof(locationManager)).IsNotNull();
            geocodingProvider.Requires(nameof(geocodingProvider)).IsNotNull();

            _locationManager = locationManager;
            _geocodingProvider = geocodingProvider;
            _locationManager.StartUpdatingLocation();

            MyLocationObservable = _locationManager.LocationChanged;
            SetPickupLocation = new RelayCommand(SetPickupLocationAction);
            SetDestinationLocation = new RelayCommand(SetDestinationLocationAction);
            RequestToRide = new RelayCommand(RequesToRideAction);
            GoToMyLocation = new RelayCommand(() => MyLocation = _locationManager.Location);
        }

        public bool IsPickupAddressLoading
        {
            get => _isPickupAddressLoading;
            set => RaiseAndSetIfChanged(ref _isPickupAddressLoading, value);
        }

        public bool IsDestinationAddressLoading
        {
            get => _isDestinationAddressLoading;
            set => RaiseAndSetIfChanged(ref _isDestinationAddressLoading, value);
        }

        public AddressMode AddressMode
        {
            get => _addressMode;
            set => RaiseAndSetIfChanged(ref _addressMode, value);
        }

        public Location MyLocation
        {
            get => _myLocation;
            private set
            {
                _myLocation = value;
                RaisePropertyChanged();
            }
        }

        public IAddressViewModel PickupAddress { get; } = new AddressViewModel();

        public IAddressViewModel DestinationAddress { get; } = new AddressViewModel();

        public IObservable<Location> MyLocationObservable { get; }

        public IObservable<Location> CameraPositionObservable { [UsedImplicitly] get; set; }

        public IObservable<bool> CameraStartMoving { get; set; }

        public ICommand SetPickupLocation { get; }

        public ICommand SetDestinationLocation { get; }

        public ICommand RequestToRide { get; }

        public ICommand GoToMyLocation { get; }

        public override Task Load()
        {
            _myLocationSubscriber = MyLocationObservable
                .Subscribe(async location =>
                {
                    if (_cameraUpdated)
                        return;

                    GoToMyLocation.Execute(null);
                    _cameraUpdated = true;
                    await CameraLocationChanged(location);
                });

            _camerStartMovingSubscriber = CameraStartMoving
                .Subscribe(moving =>
                {
                    if (AddressMode == AddressMode.Pickup)
                        IsPickupAddressLoading = true;
                    else if (AddressMode == AddressMode.Destination)
                        IsDestinationAddressLoading = true;
                });

            _cameraPositionSubscriber = CameraPositionObservable
                .Subscribe(async location => { await CameraLocationChanged(location); });
            AddressMode = AddressMode.Pickup;
            return base.Load();
        }

        private void RequesToRideAction()
        {
        }

        private void SetDestinationLocationAction()
        {
        }

        private void SetPickupLocationAction()
        {
            AddressMode = AddressMode.Destination;
        }

        private async Task CameraLocationChanged(Location location)
        {
            if (AddressMode == AddressMode.Pickup)
            {
                var address = await _geocodingProvider.ReverseGeocoding(location);
                if (address != null)
                {
                    PickupAddress.FormattedAddress = address.FormattedAddress;
                    PickupAddress.Location = address.Location;
                }
                IsPickupAddressLoading = false;
            }
            else if (AddressMode == AddressMode.Destination)
            {
                var address = await _geocodingProvider.ReverseGeocoding(location);
                if (address != null)
                {
                    DestinationAddress.FormattedAddress = address.FormattedAddress;
                    DestinationAddress.Location = address.Location;
                }
                IsDestinationAddressLoading = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _locationManager.StopUpdatingLocation();
                _cameraPositionSubscriber.Dispose();
                _myLocationSubscriber.Dispose();
                _camerStartMovingSubscriber.Dispose();
            }
        }
    }
}