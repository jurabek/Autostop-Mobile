using System;
using System.Collections.Generic;
using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core;
using Autostop.Client.Core.Enums;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Controls;
using Autostop.Client.iOS.Extensions.MainView;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using JetBrains.Annotations;
using UIKit;

namespace Autostop.Client.iOS.Controllers
{
	public partial class MainViewController : UIViewController, IScreenFor<MainViewModel>
	{
		[NotNull] private readonly IContainer _container = BootstrapperBase.Container;
		[NotNull] private readonly INavigationService _navigationService;

		private List<Binding> _bindings;
	    private bool _cameraUpdated;
        private IDisposable _cameraPositionSubscriber;
		private IDisposable _myLocationSubscriber;
	    private IDisposable _camerStartMovingSubscriber;

	    public MainViewModel ViewModel { get; set; }

		public MainViewController(IntPtr handle) : base(handle)
		{
			_navigationService = _container.Resolve<INavigationService>();
			ViewModel = _container.Resolve<MainViewModel>();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			gmsMapView.MyLocationEnabled = true;
			myLocationButton.ImageEdgeInsets = new UIEdgeInsets(10, 10, 10, 10);
			myLocationButton.TouchUpInside += MyLocationButton_TouchUpInside;
		    pickupAddressTextField.ShouldBeginEditing = PickupAddressShouldBeginEditing;
		    destinationAddressTextField.ShouldBeginEditing = DestinationAddressShouldBeginEditing;
            gmsMapView.GmsMapViewEventsToMainViewModelObservables(ViewModel);

			_bindings = new List<Binding>
			{
				this.SetBinding(
					() => pickupAddressTextField.Text,
					() => ViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay),

				this.SetBinding(
					() => destinationAddressTextField.Text,
					() => ViewModel.DestinationAddress.FormattedAddress, BindingMode.TwoWay),

				this.SetBinding(
					() => pickupAddressTextField.Mode,
					() => ViewModel.AddressMode, BindingMode.TwoWay),

				this.SetBinding(
					() => destinationAddressTextField.Mode,
					() => ViewModel.AddressMode, BindingMode.TwoWay),

				this.SetBinding(
					() => pickupAddressTextField.Loading,
					() => ViewModel.IsPickupAddressLoading, BindingMode.TwoWay),

				this.SetBinding(
					() => destinationAddressTextField.Loading,
					() => ViewModel.IsDestinationAddressLoading, BindingMode.TwoWay)
			};
            
			_myLocationSubscriber = ViewModel
                .MyLocationObservable
                .Subscribe(async location =>
			    {
				    if (_cameraUpdated)
					    return;

				    SetCameraToMyLocation();
				    _cameraUpdated = true;
				    await ViewModel.CameraLocationChanged(location);
			    });

		    _camerStartMovingSubscriber = ViewModel
                .CameraStartMoving
                .Subscribe(moving =>
		        {
		            if (ViewModel.AddressMode == AddressMode.Pickup)
		                ViewModel.IsPickupAddressLoading = true;
		            else if (ViewModel.AddressMode == AddressMode.Destination)
		                ViewModel.IsDestinationAddressLoading = true;
                });

			_cameraPositionSubscriber = ViewModel
                .CameraPosition
                .Subscribe(async location =>
			    {
                    await ViewModel.CameraLocationChanged(location);
			    });

			ViewModel.AddressMode = AddressMode.Pickup;
			setPickupLocationButton.SetCommand("TouchUpInside", ViewModel.SetPickupLocation);
		}

	    private void SetCameraToMyLocation()
	    {
	        var location = ViewModel.MyLocation;
	        var camera = CameraPosition.FromCamera(location.Latitude, location.Longitude, 17);
	        gmsMapView.Camera = camera;
	    }

        private bool DestinationAddressShouldBeginEditing(UITextField textField)
		{
			_navigationService.NavigateTo<DestinationSearchPlaceViewModel>((v, vm) =>
			{
				if (v is UIViewController searchPlaces)
				{
					var searchTextField = this.GetSearchText(vm, searchPlaces);
					searchTextField.Placeholder = "Search destination location";
				}
			});

			return false;
		}

		private bool PickupAddressShouldBeginEditing(UITextField textField)
		{
			_navigationService.NavigateTo<PickupSearchPlaceViewModel>((v, vm) =>
			{
				if (v is UIViewController searchPlaces)
				{
					var searchTextField = this.GetSearchText(vm, searchPlaces);
					searchTextField.Placeholder = "Search pickup location";
					vm.SearchText = ViewModel.PickupAddress.FormattedAddress;
				}
			});

			return false;
		}
        
		private void MyLocationButton_TouchUpInside(object sender, EventArgs e)
		{
			SetCameraToMyLocation();
		}
        
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				ViewModel.Dispose();
				myLocationButton.TouchUpInside -= MyLocationButton_TouchUpInside;
				_cameraPositionSubscriber.Dispose();
				_myLocationSubscriber.Dispose();
                _camerStartMovingSubscriber.Dispose();
			}
		}
	}
}