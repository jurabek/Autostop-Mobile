using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap.Base
{
	public abstract class ChooseAddressOnMapViewModelBase : ChooseOnMapViewModelBase
	{
		private readonly INavigationService _navigationService;
		private readonly ILocationManager _locationManager;
		private readonly IGeocodingProvider _geocodingProvider;
		private ICommand _goBack;
		private ICommand _done;
		private Address _currentAddress;

		protected ChooseAddressOnMapViewModelBase(
			INavigationService navigationService,
			ILocationManager locationManager,
			IGeocodingProvider geocodingProvider)
		{
			_navigationService = navigationService;
			_locationManager = locationManager;
			_geocodingProvider = geocodingProvider;
			MyLocationChanged = locationManager.LocationChanged;
		}

		public override ICommand Done => _done ?? (_done = new RelayCommand(() =>
		{
			SetAddress(_currentAddress);
			_navigationService.GoBack(2);
		}));

		public override ICommand GoBack => _goBack ?? (_goBack = new RelayCommand(() =>
		{
			_currentAddress = null;
			_navigationService.GoBack();
		}));

		public override async Task Load()
		{
			CameraTarget = _locationManager.LastKnownLocation;
			await CameraLocationChanged(CameraTarget);

			CameraStartMoving.Subscribe(_ => IsSearching = true);

			CameraPositionChanged.SelectMany(l => _geocodingProvider.ReverseGeocodingFromLocation(l))
				.Subscribe(address =>
				{
					SearchText = address.FormattedAddress;
					_currentAddress = address;
					IsSearching = false;
				}, ex =>
				{
					Debug.Write(ex);
					IsSearching = false;
				});
		}

		private async Task CameraLocationChanged(Location location)
		{
			var address = await _geocodingProvider.ReverseGeocodingFromLocation(location);
			if (address != null)
			{
				SearchText = address.FormattedAddress;
				_currentAddress = address;
			}
		}

		protected abstract void SetAddress(Address address);
	}
}