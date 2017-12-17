using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core;
using Autostop.Client.Core.Enums;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Controls;
using Autostop.Common.Shared.Models;
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
		private IDisposable _cameraPositionSubscriber;
		private IDisposable _myLocationSubscriber;
		private bool _cameraUpdated;

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
			myLocationButton.TouchUpInside += MyLocationButton_TouchUpInside;

			ViewModel.CameraPosition = Observable
				.FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
					e => gmsMapView.CameraPositionIdle += e,
					e => gmsMapView.CameraPositionIdle -= e)
				.Select(e => e.EventArgs.Position.Target)
				.Select(c => new Location(c.Latitude, c.Longitude));

			gmsMapView.WillMove += (sender, args) =>
			{
				if (ViewModel.AddressMode == AddressMode.Pickup)
					ViewModel.IsPickupAddressLoading = true;
				else if (ViewModel.AddressMode == AddressMode.Destination)
					ViewModel.IsDestinationAddressLoading = true;
			};

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

			// Initializing MyLocation to camera
			_myLocationSubscriber = ViewModel.MyLocationObservable.Subscribe(async location =>
			{
				if (_cameraUpdated)
					return;

				SetCameraToMyLocation();
				_cameraUpdated = true;
				await ViewModel.CameraLocationChanged(location);
			});

			_cameraPositionSubscriber = ViewModel.CameraPosition.Subscribe(async location => await ViewModel.CameraLocationChanged(location));

			ViewModel.AddressMode = AddressMode.Pickup;
			setPickupLocationButton.SetCommand("TouchUpInside", ViewModel.SetPickupLocation);

			pickupAddressTextField.ShouldBeginEditing = PickupAddressShouldBeginEditing;
			destinationAddressTextField.ShouldBeginEditing = DestinationAddressShouldBeginEditin;
		}

		private bool DestinationAddressShouldBeginEditin(UITextField textField)
		{
			_navigationService.NavigateTo<DestinationSearchPlaceViewModel>((v, vm) =>
			{
				if (v is UIViewController searchPlaces)
				{
					var searchTextField = GetSearchText(vm, searchPlaces);
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
					var searchTextField = GetSearchText(vm, searchPlaces);
					searchTextField.Placeholder = "Search pickup location";
					vm.SearchText = ViewModel.PickupAddress.FormattedAddress;
				}
			});

			return false;
		}

		private SearchPlaceTextField GetSearchText(BaseSearchPlaceViewModel vm, UIViewController searchPlaces)
		{
			searchPlaces.EdgesForExtendedLayout = UIRectEdge.None;
			searchPlaces.NavigationItem.HidesBackButton = true;
			var frame = new CGRect(0, 0, NavigationController.NavigationBar.Frame.Size.Width - 20, 30);
			var searchTextField = new SearchPlaceTextField(frame, vm, () => _navigationService.GoBack());
			searchPlaces.NavigationItem.TitleView = searchTextField;
			searchTextField.BecomeFirstResponder();
			return searchTextField;
		}

		private void MyLocationButton_TouchUpInside(object sender, EventArgs e)
		{
			SetCameraToMyLocation();
		}

		private void SetCameraToMyLocation()
		{
			var location = ViewModel.MyLocation;
			var camera = CameraPosition.FromCamera(location.Latitude, location.Longitude, 17);
			gmsMapView.Camera = camera;
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
			}
		}
	}
}