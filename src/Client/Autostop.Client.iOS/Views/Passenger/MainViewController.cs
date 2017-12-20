//using System;
//using System.Collections.Generic;
//using System.Reactive;
//using System.Reactive.Linq;
//using Autofac;
//using Autostop.Client.Abstraction;
//using Autostop.Client.Abstraction.Services;
//using Autostop.Client.Core;
//using Autostop.Client.Core.ViewModels.Passenger;
//using Autostop.Client.iOS.Extensions;
//using Autostop.Client.iOS.Extensions.MainView;
//using Autostop.Common.Shared.Models;
//using CoreGraphics;
//using CoreLocation;
//using GalaSoft.MvvmLight.Helpers;
//using Google.Maps;
//using JetBrains.Annotations;
//using UIKit;

//namespace Autostop.Client.iOS.Views.Passenger
//{
//	public partial class MainViewController : UIViewController, IScreenFor<MainViewModel>
//	{
//		private readonly IContainer _container = BootstrapperBase.Container;
//		private readonly INavigationService _navigationService;
//		[UsedImplicitly] private List<Binding> _bindings;

//		public MainViewModel ViewModel { get; set; }

//		public MainViewController(IntPtr handle) : base(handle)
//		{
//			_navigationService = _container.Resolve<INavigationService>();
//			ViewModel = _container.Resolve<MainViewModel>();
//		}

//		public override async void ViewDidLoad()
//		{
//			base.ViewDidLoad();
//			myLocationButton.ImageEdgeInsets = new UIEdgeInsets(10, 10, 10, 10);
//			myLocationButton.ToCircleButton();

//			pickupAddressTextField.ShouldBeginEditing = PickupAddressShouldBeginEditing;
//			destinationAddressTextField.ShouldBeginEditing = DestinationAddressShouldBeginEditing;

//			gmsMapView.MyLocationEnabled = true;

//			ViewModel.CameraPositionObservable = Observable
//				.FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
//					e => gmsMapView.CameraPositionIdle += e,
//					e => gmsMapView.CameraPositionIdle -= e)
//				.Do(ShowNavigationBar)
//				.Select(e => e.EventArgs.Position.Target)
//				.Select(c => new Location(c.Latitude, c.Longitude));

//			ViewModel.CameraStartMoving = Observable
//				.FromEventPattern<EventHandler<GMSWillMoveEventArgs>, GMSWillMoveEventArgs>(
//					e => gmsMapView.WillMove += e,
//					e => gmsMapView.WillMove -= e)
//				.Do(HideNavigationBar)
//				.Select(e => e.EventArgs.Gesture);

//			_bindings = new List<Binding>
//			{
//				this.SetBinding(
//					() => pickupAddressTextField.Text,
//					() => ViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay),

//				this.SetBinding(
//					() => destinationAddressTextField.Text,
//					() => ViewModel.DestinationAddress.FormattedAddress, BindingMode.TwoWay),

//				this.SetBinding(
//					() => pickupAddressTextField.Mode,
//					() => ViewModel.AddressMode, BindingMode.TwoWay),

//				this.SetBinding(
//					() => destinationAddressTextField.Mode,
//					() => ViewModel.AddressMode, BindingMode.TwoWay),

//				this.SetBinding(
//					() => pickupAddressTextField.Loading,
//					() => ViewModel.IsPickupAddressLoading, BindingMode.TwoWay),

//				this.SetBinding(
//					() => destinationAddressTextField.Loading,
//					() => ViewModel.IsDestinationAddressLoading, BindingMode.TwoWay),

//				this.SetBinding(
//					() => gmsMapView.Camera,
//					() => ViewModel.MyLocation, BindingMode.TwoWay)
//					.ConvertTargetToSource(location => CameraPosition.FromCamera(location.Latitude, location.Longitude, 17))
//			};

//			this.BindCommand(setPickupLocationButton, ViewModel.SetPickupLocation);
//			this.BindCommand(myLocationButton, ViewModel.GoToMyLocation);

//			var marker = new Marker();
//			marker.Position = new CLLocationCoordinate2D(38.578545, 68.741587);
//			marker.Icon = UIImage.FromFile("car.png");
//			marker.Map = gmsMapView;
//			await ViewModel.Load();
//		}

//		private void ShowNavigationBar(EventPattern<GMSCameraEventArgs> eventPattern)
//		{
//			//NavigationController.NavigationBarHidden = false;
//			myLocationButton.Hidden = false;
//			UIView.Animate(0.3, () =>
//			{
//				setPickupLocationButton.Transform = CGAffineTransform.MakeIdentity();
//				setPickupLocationButton.Alpha = 1;
//			});
//		}

//		private void HideNavigationBar(EventPattern<GMSWillMoveEventArgs> eventPattern)
//		{
//			//NavigationController.NavigationBarHidden = true;
//			myLocationButton.Hidden = true;
//			UIView.Animate(0.3, () =>
//			{
//				setPickupLocationButton.Transform = CGAffineTransform.MakeScale((nfloat)0.1, 1);
//				setPickupLocationButton.Alpha = 0;
//			});
//		}

//		private bool DestinationAddressShouldBeginEditing(UITextField textField)
//		{
//			_navigationService.NavigateTo<DestinationSearchPlaceViewModel>((view, vm) =>
//			{
//				if (view is UIViewController searchPlaces)
//				{
//					var searchTextField = this.GetSearchText(vm, searchPlaces);
//					searchTextField.Placeholder = "Search destination location";
//				}
//			});

//			return false;
//		}

//		private bool PickupAddressShouldBeginEditing(UITextField textField)
//		{
//			_navigationService.NavigateTo<PickupSearchPlaceViewModel>((view, vm) =>
//			{
//				if (view is UIViewController searchPlaces)
//				{
//					var searchTextField = this.GetSearchText(vm, searchPlaces);
//					searchTextField.Placeholder = "Search pickup location";
//					vm.SearchText = ViewModel.PickupAddress.FormattedAddress;
//				}
//			});

//			return false;
//		}

//		protected override void Dispose(bool disposing)
//		{
//			base.Dispose(disposing);
//			if (disposing)
//			{
//				ViewModel.Dispose();
//			}
//		}
//	}
//}