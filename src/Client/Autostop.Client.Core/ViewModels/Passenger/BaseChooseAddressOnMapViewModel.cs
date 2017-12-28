using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public abstract class BaseChooseAddressOnMapViewModel : BaseChooseOnMapViewModel
    {
        private readonly ILocationManager _locationManager;
        private readonly IGeocodingProvider _geocodingProvider;
        private Address _currentAddress;

        protected BaseChooseAddressOnMapViewModel(
            ILocationManager locationManager,
            IGeocodingProvider geocodingProvider)
        {
            _locationManager = locationManager;
            _geocodingProvider = geocodingProvider;
            MyLocationObservable = locationManager.LocationChanged;
            Done = new RelayCommand(DoneAction);
            GoBack = new RelayCommand(GoBackAction);
        }

        private void GoBackAction()
        {
            
        }

        private void DoneAction()
        {
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
                _currentAddress = address;
            }
            IsSearching = false;
        }
    }
}