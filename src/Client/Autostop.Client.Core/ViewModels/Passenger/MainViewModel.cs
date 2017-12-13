using System;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ILocationManager _locationManager;
        private bool _hasPickupLocation;

        public MainViewModel(ILocationManager locationManager)
        {
            _locationManager = locationManager;
            _locationManager.StartUpdatingLocation();

            SetPickupLocation = new RelayCommand(() =>
            {
                HasPickupLocation = true;
            });

            SetDestination = new RelayCommand(() =>
            {
            });

            RequestToRide = new RelayCommand(() =>
            {
            });

            CurrentLocation = _locationManager.LocationChanged;
        }

        public bool HasPickupLocation
        {
            get => _hasPickupLocation;
            set => RaiseAndSetIfChanged(ref _hasPickupLocation, value);
        }

        public IObservable<Location> CurrentLocation { get; }

        #region Commands
        public ICommand SetPickupLocation { get; }

        public ICommand SetDestination { get; }

        public ICommand RequestToRide { get; }

        #endregion

        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _locationManager.StopUpdatingLocation();
            }
            base.Dispose(disposing);
        }
    }
}
