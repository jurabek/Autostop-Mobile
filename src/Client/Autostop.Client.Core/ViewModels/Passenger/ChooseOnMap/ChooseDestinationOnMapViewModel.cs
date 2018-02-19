using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap
{
	public sealed class ChooseDestinationOnMapViewModel : ChooseOnMapViewModelBase
	{
		private readonly Subject<Address> _selectedAddress = new Subject<Address>();
		private readonly INavigationService _navigationService;
		private readonly IGeocodingProvider _geocodingProvider;
		private Address _currentAddress;
	    private ICommand _goBack;

        public override IObservable<Address> SelectedAddress => _selectedAddress;

		public ChooseDestinationOnMapViewModel(
			INavigationService navigationService,
			IGeocodingProvider geocodingProvider)
		{
			_navigationService = navigationService;
			_geocodingProvider = geocodingProvider;
		}

		private ICommand _done;
		public override ICommand Done => _done ?? (_done = new RelayCommand(() =>
			                                 {
				                                 _selectedAddress.OnNext(_currentAddress);
				                                 _navigationService.NavigaeToRoot();
											 }));

		public override ICommand GoBack => _goBack ?? (_goBack = new RelayCommand(() => _navigationService.GoBack()));

		public override Task Load()
		{
			CameraStartMoving.Subscribe(_ => IsSearching = true);

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
