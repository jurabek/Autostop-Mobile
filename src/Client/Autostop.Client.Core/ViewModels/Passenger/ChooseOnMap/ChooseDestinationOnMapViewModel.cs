using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap.Base;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap
{
	public class ChooseDestinationOnMapViewModel : ChooseOnMapViewModelBase
	{
		private readonly Subject<Address> _selectedAddress = new Subject<Address>();
	    private readonly ISchedulerProvider _schedulerProvider;
	    private readonly INavigationService _navigationService;
		private readonly IGeocodingProvider _geocodingProvider;
		private Address _currentAddress;
	    private ICommand _goBack;
	    private ICommand _done;

        public override IObservable<Address> SelectedAddress => _selectedAddress;

		public ChooseDestinationOnMapViewModel(
            ISchedulerProvider schedulerProvider,
			INavigationService navigationService,
			IGeocodingProvider geocodingProvider)
		{
		    _schedulerProvider = schedulerProvider;
		    _navigationService = navigationService;
			_geocodingProvider = geocodingProvider;
		}

		public override ICommand Done => _done ?? (_done = new RelayCommand(() =>
		    {
			    _selectedAddress.OnNext(_currentAddress);
			    _navigationService.NavigateToRoot();
		    }));

		public override ICommand GoBack => _goBack ?? (_goBack = new RelayCommand(() =>
		    {
		        _navigationService.GoBack();
		    }));

		public override Task Load()
		{
			CameraStartMoving
                .Do(_ => IsSearching = true)
                .Subscribe();

            CameraPositionChanged
                .ObserveOn(_schedulerProvider.DefaultScheduler)
                .Do(async location =>
			    {
			        var address = await _geocodingProvider.ReverseGeocodingFromLocation(location);
			        SearchText = address.FormattedAddress;
			        _currentAddress = address;

			        IsSearching = false;
			    }).Subscribe();

			return base.Load();
		}
	}
}
