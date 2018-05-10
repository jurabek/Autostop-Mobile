using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Publishers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap.Base;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap
{
	public class ChooseDestinationOnMapViewModel : ChooseOnMapViewModelBase
	{
		private readonly ISchedulerProvider _schedulerProvider;
		private readonly INavigationService _navigationService;
		private readonly IGeocodingProvider _geocodingProvider;
		private readonly ISelectedDestinationByMapPublisher _destinationByMapPublisher;
		private Address _currentAddress;
		private ICommand _goBack;
		private ICommand _done;

		public ChooseDestinationOnMapViewModel(
			ISchedulerProvider schedulerProvider,
			INavigationService navigationService,
			IGeocodingProvider geocodingProvider,
			ISelectedDestinationByMapPublisher destinationByMapPublisher)
		{
			_schedulerProvider = schedulerProvider;
			_navigationService = navigationService;
			_geocodingProvider = geocodingProvider;
			_destinationByMapPublisher = destinationByMapPublisher;
		}

		public override ICommand Done => _done ?? (_done = new RelayCommand(() =>
		{
			_destinationByMapPublisher.Publish(_currentAddress);
			_navigationService.NavigateToRoot();
		}));

		public override ICommand GoBack => _goBack ?? (_goBack = new RelayCommand(() =>
		{
			_navigationService.GoBack();
		}));

		public override Task Load()
		{
			CameraStartMoving.Subscribe(_ => IsSearching = true);

			CameraPositionChanged.SelectMany(l => _geocodingProvider.ReverseGeocodingFromLocation(l))
				.ObserveOn(_schedulerProvider.DefaultScheduler)
				.Subscribe(address =>
				{
					SearchText = address.FormattedAddress;
					_currentAddress = address;
					IsSearching = false;
				}, ex =>
				{
					Debug.WriteLine(ex);
					IsSearching = false;
				});

			return base.Load();
		}
	}
}
