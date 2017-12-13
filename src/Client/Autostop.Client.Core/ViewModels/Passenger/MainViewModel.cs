using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;
using Conditions;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class MainViewModel : BaseViewModel, IMainViewModel
	{
		private readonly ILocationManager _locationManager;
		private readonly IGeocodingProvider _geocodingProvider;
		private bool _hasPickupLocation;
		private bool _isPickupLocationLoading;

		public MainViewModel(
			ILocationManager locationManager,
			IGeocodingProvider geocodingProvider)
		{
			locationManager.Requires(nameof(locationManager)).IsNotNull();
			geocodingProvider.Requires(nameof(geocodingProvider)).IsNotNull();

			_locationManager = locationManager;
			_geocodingProvider = geocodingProvider;

			CurrentLocation = locationManager.LocationChanged;
			SetPickupLocation = new RelayCommand(SetPickupLocationAction);
			SetDestinationLocation = new RelayCommand(SetDestinationLocationAction);
			RequestToRide = new RelayCommand(RequesToRideAction);
			
			CurrentLocation.Subscribe(
				async location => await CurrentLocationChanged(location));

			_locationManager.StartUpdatingLocation();
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

		private async Task CurrentLocationChanged(Location location)
		{
			IsPickupLocationLoading = true;
			var coordinate = location.Coordinate;
			var result = await _geocodingProvider.ReverseGeocoding(coordinate.Latitude, coordinate.Latitude);
			var address = result.Results.FirstOrDefault();
			PickupLocation.FormattedAddress = address.FormattedAddress;
			PickupLocation.PlaceId = address.PlaceId;
			IsPickupLocationLoading = false;
		}

		public bool IsPickupLocationLoading
		{
			get => _isPickupLocationLoading;
			set => RaiseAndSetIfChanged(ref _isPickupLocationLoading, value);
		}

		public bool HasPickupLocation
		{
			get => _hasPickupLocation;
			set => RaiseAndSetIfChanged(ref _hasPickupLocation, value);
		}


		public AddressViewModel PickupLocation { get; } = new AddressViewModel();

		public AddressViewModel DestinationLocation { get; } = new AddressViewModel();

		public IObservable<Location> CurrentLocation { get; }
		
		public ICommand SetPickupLocation { get; }

		public ICommand SetDestinationLocation { get; }

		public ICommand RequestToRide { get; }
		
		public override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				_locationManager.StopUpdatingLocation();
			}
		}
	}
}
