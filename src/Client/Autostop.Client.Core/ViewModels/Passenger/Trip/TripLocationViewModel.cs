using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.Trip
{
	/// <summary>
	/// TODO: we should refactor this class
	/// </summary>
    public class TripLocationViewModel : BaseViewModel, ITripLocationViewModel
    {
        private readonly Subject<bool> _pickupLocationChanged = new Subject<bool>();
        private readonly IBaseLocationEditorViewModel _pickupLocationEditorViewModel;
        private readonly IBaseLocationEditorViewModel _destinationLocationEditorViewModel;
        private readonly INavigationService _navigationService;

        private ICommand _navigateWhereTo;
        private ICommand _navigatePickupSearch;
        private bool _canRequest;


        public TripLocationViewModel(
            ISchedulerProvider schedulerProvider,
            INavigationService navigationService,
            ISearchPlaceViewModelFactory searchPlaceViewModelFactory)
        {
            _pickupLocationEditorViewModel = searchPlaceViewModelFactory.GetPickupLocationEditorViewModel();
            _destinationLocationEditorViewModel = searchPlaceViewModelFactory.GetDestinationLocationEditorViewModel();
            _navigationService = navigationService;
            
            _pickupLocationEditorViewModel.SelectedAddress
                .ObserveOn(schedulerProvider.SynchronizationContextScheduler)
                .Subscribe(address =>
                {
                    PickupAddress.SetAddress(address);
                    _pickupLocationChanged.OnNext(true);
                    _navigationService.GoBack();
                });

            _destinationLocationEditorViewModel.SelectedAddress
                .ObserveOn(schedulerProvider.SynchronizationContextScheduler)
                .Subscribe(address =>
                {
                    DestinationAddress.SetAddress(address);
                    CanRequest = true;
                    _navigationService.GoBack();
                });
        }

        public IObservable<bool> PickupLocationChanged => _pickupLocationChanged;

        public IAddressModel PickupAddress { get; } = new AddressModel();

        public IAddressModel DestinationAddress { get; } = new AddressModel();

        public ICommand NavigateToPickupSearch => _navigatePickupSearch ?? ( _navigatePickupSearch = new RelayCommand(() =>
            {
                _navigationService.NavigateToSearchView(_pickupLocationEditorViewModel as PickupLocationEditorViewModel);
                _pickupLocationEditorViewModel.SearchText = PickupAddress.FormattedAddress;
            }));

        public ICommand NavigateToWhereTo => _navigateWhereTo ?? (_navigateWhereTo = new RelayCommand(() =>
            {
                _navigationService.NavigateToSearchView(_destinationLocationEditorViewModel as DestinationLocationEditorViewModel);
            }));
        
        public bool CanRequest
        {
            get => _canRequest;
            set => RaiseAndSetIfChanged(ref _canRequest, value);
        }
    }
}
