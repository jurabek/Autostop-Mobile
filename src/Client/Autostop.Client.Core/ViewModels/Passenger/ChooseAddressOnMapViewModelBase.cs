using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

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
            Done = new RelayCommand(DoneAction);
            GoBack = new RelayCommand(GoBackAction);
        }

        private void GoBackAction()
        {
            _navigationService.GoBack();
        }

        private void DoneAction()
        {
            _navigationService.GoBack();
            _navigationService.GoBack();
        }

        public override async Task Load()
        {
            CameraTarget = _locationManager.Location;
            await CameraLocationChanged(_locationManager.Location);

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