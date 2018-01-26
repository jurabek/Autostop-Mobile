using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap
{
	public sealed class ChooseDestinationOnMapViewModel : ChooseOnMapViewModelBase
	{
		public ITripLocationViewModel TripLocationViewModel { get; }

		private readonly INavigationService _navigationService;
		private readonly IGeocodingProvider _geocodingProvider;
		private Address _currentAddress;

		public ChooseDestinationOnMapViewModel(
			ITripLocationViewModel tripLocationViewModel,
			INavigationService navigationService,
			IGeocodingProvider geocodingProvider)
		{
			TripLocationViewModel = tripLocationViewModel;
			_navigationService = navigationService;
			_geocodingProvider = geocodingProvider;
		}

		private ICommand _done;
		public override ICommand Done => _done ?? (_done = new RelayCommand(() =>
				{
					TripLocationViewModel.DestinationAddress.SetAddress(_currentAddress);
					_navigationService.NavigaeToRoot();
				}));

		private ICommand _goBack;
		public override ICommand GoBack => _goBack ?? (_goBack = new RelayCommand(
			() => _navigationService.GoBack()));

		public override Task Load()
		{
			CameraTarget = TripLocationViewModel.PickupAddress.Location;
			CameraStartMoving
				.Do(_ => IsSearching = true)
				.Subscribe();

			CameraPositionObservable
				.Subscribe(async location =>
				{
					var address = await _geocodingProvider.ReverseGeocodingFromLocation(location);
					SearchText = address.FormattedAddress;
					_currentAddress = address;
					IsSearching = false;
				});

			return base.Load();
		}
	}
}
