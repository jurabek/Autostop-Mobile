using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Client.Core.Enums;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using Autostop.Common.Shared.Models;
using Conditions;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class MainViewModel : BaseViewModel, IMainViewModel
	{
		private readonly IGeocodingProvider _geocodingProvider;
		private readonly INavigationService _navigationService;
		private AddressMode _addressMode;
		private IDisposable _cameraPositionSubscriber;
		private IDisposable _camerStartMovingSubscriber;
		private bool _isDestinationAddressLoading;
		private bool _isPickupAddressLoading;
		private IDisposable _myLocationSubscriber;
		private ObservableCollection<DriverLocation> _onlineDrivers;
		private Location _cameraTarget;

		public MainViewModel(
			INavigationService navigationService,
			IGeocodingProvider geocodingProvider,
			ILocationManager locationManager)
		{
			geocodingProvider.Requires(nameof(geocodingProvider)).IsNotNull();
			locationManager.Requires(nameof(locationManager)).IsNotNull();
			
			_navigationService = navigationService;
			_geocodingProvider = geocodingProvider;
			_locationManager = locationManager;
			locationManager.StartUpdatingLocation();

			MyLocationObservable = locationManager.LocationChanged;
			SetPickupLocation = new RelayCommand(SetPickupLocationAction);
			NavigateToDestinationSearch = new RelayCommand(NavigateToDistinationSearchViewModel);
			NavigateToPickupSearch = new RelayCommand(NavigateToPickupSearchViewModel);
			GoToMyLocation = new RelayCommand(GoToMyLocationAction);
		}

		private void GoToMyLocationAction()
		{
			CameraTarget = _locationManager.Location;
		}

		public IObservable<Location> MyLocationObservable { get; }

		public IObservable<Location> CameraPositionObservable { [UsedImplicitly] get; set; }

		public IObservable<bool> CameraStartMoving { get; set; }

		public Location CameraTarget
		{
			get => _cameraTarget;
			set
			{
				_cameraTarget = value;
				RaisePropertyChanged();
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

		public AddressMode AddressMode
		{
			get => _addressMode;
			set => RaiseAndSetIfChanged(ref _addressMode, value);
		}
		
		public IAddressViewModel PickupAddress { get; } = new AddressViewModel();

		public IAddressViewModel DestinationAddress { get; } = new AddressViewModel();

		public ObservableCollection<DriverLocation> OnlineDrivers
		{
			get => _onlineDrivers;
			set => RaiseAndSetIfChanged(ref _onlineDrivers, value);
		}

		public ICommand GoToMyLocation { get; }

		public ICommand SetPickupLocation { get; }

		public ICommand NavigateToPickupSearch { get;  }

		public ICommand NavigateToDestinationSearch { get; }

        private bool _cameraUpdated;
		private readonly ILocationManager _locationManager;

		public override Task Load()
		{
			_myLocationSubscriber = MyLocationObservable
				.Subscribe(async location =>
				{
					if(_cameraUpdated)
						return;

					CameraTarget = _locationManager.Location;
					await CameraLocationChanged(location);
					_cameraUpdated = true;
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
				.Subscribe(async location =>
				{
					await CameraLocationChanged(location);
				});

			AddressMode = AddressMode.Pickup;
			OnlineDrivers = new ObservableCollection<DriverLocation>(MockData.AvailableDrivers);

			return base.Load();
		}
		
		private void SetPickupLocationAction()
		{
			AddressMode = AddressMode.Destination;
		}

		private async Task CameraLocationChanged(Location location)
		{
			var address = await _geocodingProvider.ReverseGeocodingFromLocation(location);
			if (address != null)
			{
				switch (AddressMode)
				{
					case AddressMode.Pickup:
						PickupAddress.SetAddress(address);
						break;
					case AddressMode.Destination:
						DestinationAddress.SetAddress(address);
						break;
				}
			}
			IsDestinationAddressLoading = false;
			IsPickupAddressLoading = false;
		}

		private async Task ChangeAddressAndMoveCameraToLocation(string placeId)
		{
			var address = await _geocodingProvider.ReverseGeocodingFromPlaceId(placeId);
			if (address != null)
			{
				switch (AddressMode)
				{
					case AddressMode.Pickup:
						PickupAddress.SetAddress(address);
						break;
					case AddressMode.Destination:
						DestinationAddress.SetAddress(address);
						break;
				}
				CameraTarget = address.Location;
			}
		}

		private void NavigateToPickupSearchViewModel()
		{
			_navigationService.NavigateToSearchView<PickupSearchPlaceViewModel>(vm =>
			{
				vm.SearchText = PickupAddress.FormattedAddress;
				vm.ObservablePropertyChanged(() => vm.SelectedSearchResult)
					.OfType<AutoCompleteResultModel>()
					.Subscribe(async selectedResult =>
					{
						await ChangeAddressAndMoveCameraToLocation(selectedResult.PlaceId);
						_navigationService.GoBack();
					});
			});
		}

		private void NavigateToDistinationSearchViewModel()
		{
			_navigationService.NavigateToSearchView<DestinationSearchPlaceViewModel>(vm =>
			{
				vm.SearchText = null;
				vm.ObservablePropertyChanged(() => vm.SelectedSearchResult)
					.OfType<AutoCompleteResultModel>()
					.Subscribe(async selectedResult =>
					{
						await ChangeAddressAndMoveCameraToLocation(selectedResult.PlaceId);
						_navigationService.GoBack();
					});
			});
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				//_locationManager.StopUpdatingLocation();
				_cameraPositionSubscriber.Dispose();
				_myLocationSubscriber.Dispose();
				_camerStartMovingSubscriber.Dispose();
			}
		}
	}
}