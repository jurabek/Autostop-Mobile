using System;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    [UsedImplicitly]
    public sealed class ChooseDestinationOnMapViewModel : BaseChooseOnMapViewModel
    {
        public IRideViewModel RideViewModel { get; }

        private readonly INavigationService _navigationService;
        private readonly IGeocodingProvider _geocodingProvider;
        private Address _currentAddress;

        public ChooseDestinationOnMapViewModel(
            IRideViewModel rideViewModel,
            INavigationService navigationService,
            IGeocodingProvider geocodingProvider)
        {
            RideViewModel = rideViewModel;
            _navigationService = navigationService;
            _geocodingProvider = geocodingProvider;
            GoBack = new RelayCommand(GoBackExecute);
            Done = new RelayCommand(() =>
            {
                RideViewModel.DestinationAddress.SetAddress(_currentAddress);
                _navigationService.GoBack();
                _navigationService.GoBack();
            });
        }

        private void GoBackExecute()
        {
            _navigationService.GoBack();
        }

        public override Task Load()
        {
            CameraTarget = RideViewModel.PickupAddress.Location;

            CameraStartMoving
                .Subscribe(moving =>
                {
                    IsSearching = true;
                });

            CameraPositionObservable
                .Subscribe(async location =>
                {
                    var address = await _geocodingProvider.ReverseGeocodingFromLocation(location);
                    if (address != null)
                    {
                        SearchText = address.FormattedAddress;
                        _currentAddress = address;
                    }
                    IsSearching = false;
                });

            return base.Load();
        }
    }
}
