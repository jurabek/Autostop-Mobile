using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Autofac;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core;
using Autostop.Client.Core.Enums;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using Autostop.Client.iOS.Extensions.MainView;
using Autostop.Client.iOS.UI;
using Autostop.Common.Shared.Models;
using CoreGraphics;
using CoreLocation;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using UIKit;

namespace Autostop.Client.iOS.Views.Passenger
{
	public class TestMainViewController : UIViewController
	{
		private readonly AutostopMapView _mapView = new AutostopMapView { TranslatesAutoresizingMaskIntoConstraints = false };
		private readonly PickupAddressTextField _pickupAddressTextField = new PickupAddressTextField();
		private readonly DestinationAddressTextField _destinationAddressTextField = new DestinationAddressTextField();
		private readonly MyLocationButton _myLocationButton = new MyLocationButton { TranslatesAutoresizingMaskIntoConstraints = false };
		private readonly UIButton _setPickupButton = new UIButton { BackgroundColor = Colors.PickupButtonColor, TranslatesAutoresizingMaskIntoConstraints = false };
		private readonly UIImageView _centerPinImageView = new UIImageView(Icons.Pin) { ContentMode = UIViewContentMode.ScaleAspectFit, TranslatesAutoresizingMaskIntoConstraints = false };
		private readonly IContainer _container = BootstrapperBase.Container;
		private readonly INavigationService _navigationService;
		private List<Binding> _bindings;
		private UIStackView _addresseStackView;

		public TestMainViewController()
		{
			_navigationService = _container.Resolve<INavigationService>();
			ViewModel = _container.Resolve<MainViewModel>();
		}

		public MainViewModel ViewModel { get; set; }

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			_mapView.MyLocationEnabled = true;
			_setPickupButton.Layer.CornerRadius = 20;
			_setPickupButton.SetTitle("SET PICKUP LOCATION", UIControlState.Normal);
			_addresseStackView = new UIStackView(new UIView[] { _pickupAddressTextField, _destinationAddressTextField })
			{
				Axis = UILayoutConstraintAxis.Vertical,
				TranslatesAutoresizingMaskIntoConstraints = false,
				Distribution = UIStackViewDistribution.FillEqually,
				Spacing = 5
			};

			_pickupAddressTextField.ShouldBeginEditing = PickupAddressShouldBeginEditing;
			_destinationAddressTextField.ShouldBeginEditing = DestinationAddressShouldBeginEditing;


			ViewModel.CameraPositionObservable = Observable
				.FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
					e => _mapView.CameraPositionIdle += e,
					e => _mapView.CameraPositionIdle -= e)
				.Do(ShowNavigationBar)
				.Select(e => e.EventArgs.Position.Target)
				.Select(c => new Location(c.Latitude, c.Longitude));

			ViewModel.CameraStartMoving = Observable
				.FromEventPattern<EventHandler<GMSWillMoveEventArgs>, GMSWillMoveEventArgs>(
					e => _mapView.WillMove += e,
					e => _mapView.WillMove -= e)
				.Do(HideNavigationBar)
				.Select(e => e.EventArgs.Gesture);

			AddCommands();
			SetupBindings();
			AddSubViews();
			SetupConstraints();
			var marker = new Marker();
			marker.Position = new CLLocationCoordinate2D(38.578545, 68.741587);
			marker.Icon = UIImage.FromFile("car.png");
			marker.Map = _mapView;
			await ViewModel.Load();
		}

		private void AddCommands()
		{
			this.BindCommand(_setPickupButton, ViewModel.SetPickupLocation);
			this.BindCommand(_myLocationButton, ViewModel.GoToMyLocation);
		}

		private void ShowNavigationBar(EventPattern<GMSCameraEventArgs> eventPattern)
		{
			//NavigationController.NavigationBarHidden = false;
			_myLocationButton.Hidden = false;
			UIView.Animate(0.3, () =>
			{
				_setPickupButton.Transform = CGAffineTransform.MakeIdentity();
				_setPickupButton.Alpha = 1;
			});
		}

		private void HideNavigationBar(EventPattern<GMSWillMoveEventArgs> eventPattern)
		{
			//NavigationController.NavigationBarHidden = true;
			_myLocationButton.Hidden = true;
			UIView.Animate(0.3, () =>
			{
				_setPickupButton.Transform = CGAffineTransform.MakeScale((nfloat)0.1, 1);
				_setPickupButton.Alpha = 0;
			});
		}

		private bool DestinationAddressShouldBeginEditing(UITextField textField)
		{
			_navigationService.NavigateTo<DestinationSearchPlaceViewModel>((view, vm) =>
			{
				if (view is UIViewController searchPlaces)
				{
					var searchTextField = this.GetSearchText(vm, searchPlaces);
					searchTextField.Placeholder = "Search destination location";
				}
			});

			return false;
		}

		private bool PickupAddressShouldBeginEditing(UITextField textField)
		{
			_navigationService.NavigateTo<PickupSearchPlaceViewModel>((view, vm) =>
			{
				if (view is UIViewController searchPlaces)
				{
					var searchTextField = this.GetSearchText(vm, searchPlaces);
					searchTextField.Placeholder = "Search pickup location";
					vm.SearchText = ViewModel.PickupAddress.FormattedAddress;
				}
			});

			return false;
		}


		private void SetupBindings()
		{
			_bindings = new List<Binding>
			{
				this.SetBinding(
					() => _pickupAddressTextField.Text,
					() => ViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay),

				this.SetBinding(
					() => _destinationAddressTextField.Text,
					() => ViewModel.DestinationAddress.FormattedAddress, BindingMode.TwoWay),

				this.SetBinding(
					() => _pickupAddressTextField.Mode,
					() => ViewModel.AddressMode, BindingMode.TwoWay),

				//this.SetBinding(
				//	() => _destinationAddressTextField.Mode,
				//	() => ViewModel.AddressMode, BindingMode.TwoWay),

				//this.SetBinding(
				//	() => _pickupAddressTextField.Loading,
				//	() => ViewModel.IsPickupAddressLoading, BindingMode.TwoWay),

				this.SetBinding(
					() => _destinationAddressTextField.Loading,
					() => ViewModel.IsDestinationAddressLoading, BindingMode.TwoWay),

				this.SetBinding(
					() => _mapView.Camera,
					() => ViewModel.MyLocation, BindingMode.TwoWay)
					.ConvertTargetToSource(location => CameraPosition.FromCamera(location.Latitude, location.Longitude, 17))
			};
		}

		private void AddSubViews()
		{
			View.AddSubview(_mapView);
			View.AddSubview(_addresseStackView);
			View.AddSubview(_centerPinImageView);
			View.AddSubview(_setPickupButton);
			View.AddSubview(_myLocationButton);
		}

		private void SetupConstraints()
		{
			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_mapView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
				_mapView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
				_mapView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor),
				_mapView.HeightAnchor.ConstraintEqualTo(View.HeightAnchor)
			});

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_addresseStackView.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor, 6),
				_addresseStackView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 10),
				_addresseStackView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -10),
				_addresseStackView.HeightAnchor.ConstraintEqualTo(80)
			});

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_myLocationButton.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -10),
				_myLocationButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -150),
				_myLocationButton.WidthAnchor.ConstraintEqualTo(40),
				_myLocationButton.HeightAnchor.ConstraintEqualTo(40),
			});

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_centerPinImageView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
				_centerPinImageView.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor),
				_centerPinImageView.WidthAnchor.ConstraintEqualTo(46),
				_centerPinImageView.HeightAnchor.ConstraintEqualTo(60)
			});

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_setPickupButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
				_setPickupButton.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor),
				_setPickupButton.WidthAnchor.ConstraintEqualTo(305),
				_setPickupButton.HeightAnchor.ConstraintEqualTo(35),
			});
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				ViewModel.Dispose();
			}
		}
	}
}
