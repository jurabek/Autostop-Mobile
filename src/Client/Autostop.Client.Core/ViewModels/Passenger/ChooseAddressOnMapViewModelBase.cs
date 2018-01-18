using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public abstract class ChooseAddressOnMapViewModelBase : ChooseOnMapViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILocationManager _locationManager;
        private readonly IGeocodingProvider _geocodingProvider;
        private Address _currentAddress;

        protected ChooseAddressOnMapViewModelBase(
            INavigationService navigationService,
            ILocationManager locationManager,
            IGeocodingProvider geocodingProvider)
        {
            _navigationService = navigationService;
            _locationManager = locationManager;
            _geocodingProvider = geocodingProvider;
            MyLocationObservable = locationManager.LocationChanged;
        }

        [UsedImplicitly] private ICommand _done;
        public override ICommand Done => _done ?? new RelayCommand(
            () =>
            {
                _navigationService.GoBack();
                _navigationService.GoBack();
            });

        [UsedImplicitly] private ICommand _goBack;
        public override ICommand GoBack => _goBack ?? new RelayCommand(
            () => _navigationService.GoBack());

        public override async Task Load()
        {
            CameraTarget = _locationManager.LastKnownLocation;
            await CameraLocationChanged(_locationManager.LastKnownLocation);

            CameraStartMoving.Do(_ => IsSearching = true)
                .Subscribe();

            CameraPositionObservable.Subscribe(async location => await CameraLocationChanged(location));
        }

        private async Task CameraLocationChanged(Location location)
        {
            var address = await _geocodingProvider.ReverseGeocodingFromLocation(location);
            if (address != null)
            {
                SearchText = address.FormattedAddress;
                _currentAddress = new Address
                {
                    FormattedAddress = address.FormattedAddress,
                    Location = location
                };
            }
            IsSearching = false;
        }
    }
}