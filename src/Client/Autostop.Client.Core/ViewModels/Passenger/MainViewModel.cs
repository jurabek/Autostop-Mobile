using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Client.Core.Enums;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;
using Conditions;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class MainViewModel : BaseViewModel, IMainViewModel
	{
		private readonly ILocationManager _locationManager;
		private readonly IGeocodingProvider _geocodingProvider;
		private bool _hasPickupLocation;
		private bool _isPickupAddressLoading;
		private bool _isDestinationAddressLoading;
		private AddressMode _addressMode = AddressMode.Pickup;

		public MainViewModel(
			ILocationManager locationManager,
			IGeocodingProvider geocodingProvider)
		{
			locationManager.Requires(nameof(locationManager)).IsNotNull();
			geocodingProvider.Requires(nameof(geocodingProvider)).IsNotNull();

			_locationManager = locationManager;
			_geocodingProvider = geocodingProvider;
			_locationManager.StartUpdatingLocation();

			CurrentLocationObservable = _locationManager.LocationChanged;
			SetPickupLocation = new RelayCommand(SetPickupLocationAction);
			SetDestinationLocation = new RelayCommand(SetDestinationLocationAction);
			RequestToRide = new RelayCommand(RequesToRideAction);

			CameraLocationObservable.Subscribe(async location => await CameraLocationChanged(location));
		}

		private void RequesToRideAction()
		{
		}

		private void SetDestinationLocationAction()
		{
		}

		private void SetPickupLocationAction()
		{
			HasPickupLocation = true;
		}

		private async Task CameraLocationChanged(Location location)
		{
			if (AddressMode == AddressMode.Pickup)
			{
				IsPickupAddressLoading = true;
				var address = await _geocodingProvider.ReverseGeocoding(location);
				PickupAddress.FormattedAddress = address.FormattedAddress;
				PickupAddress.Location = address.Location;
				IsPickupAddressLoading = false;
			}
			else if (AddressMode == AddressMode.Destination)
			{
				IsDestinationAddressLoading = true;
				IsDestinationAddressLoading = false;
			}
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

		public bool HasPickupLocation
		{
			get => _hasPickupLocation;
			set => RaiseAndSetIfChanged(ref _hasPickupLocation, value);
		}

		public AddressMode AddressMode
		{
			get => _addressMode;
			set => RaiseAndSetIfChanged(ref _addressMode, value);
		}

	    public IAddressViewModel PickupAddress { get; } = new AddressViewModel();

	    public IAddressViewModel DestinationAddress { get; } = new AddressViewModel();

	    public IObservable<Location> CurrentLocationObservable { get; }

		public IObservable<Location> CameraLocationObservable { [UsedImplicitly] get; set; }
		
		public ICommand SetPickupLocation { get; }

		public ICommand SetDestinationLocation { get; }

		public ICommand RequestToRide { get; }
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				_locationManager.StopUpdatingLocation();
			}
		}
	}
}
