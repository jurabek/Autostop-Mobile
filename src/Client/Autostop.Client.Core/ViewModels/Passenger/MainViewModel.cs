using System;
using System.Linq;
using System.Reactive.Linq;
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
		private AddressMode _addressMode;

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
			AddressMode = AddressMode.Destination;
		}

		public async Task CameraLocationChanged(Location location)
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

	    [UsedImplicitly] public IObservable<Location> MyLocationObservable { get; }

		public IObservable<Location> CameraPosition { [UsedImplicitly] get; set; }

		public IObservable<Location> CameraStartMoving { get; set; }

		public Location MyLocation => _locationManager.Location;

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
