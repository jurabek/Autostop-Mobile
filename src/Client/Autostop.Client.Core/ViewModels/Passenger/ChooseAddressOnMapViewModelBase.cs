using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private ICommand _done;
        private ICommand _goBack;

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

        public override ICommand  Done => _done ?? new RelayCommand(
            () => 
            {
                _navigationService.GoBack();
                _navigationService.GoBack();
            });

        public override ICommand GoBack => _goBack ?? new RelayCommand(
            () => _navigationService.GoBack());

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