using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Facades;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using Autostop.Client.iOS.UI;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Providers;
using CoreLocation;
using UIKit;
using Location = Autostop.Common.Shared.Models.Location;
using MapView = Autostop.Client.iOS.UI.MapView;

namespace Autostop.Client.iOS.Views.Passenger
{
	[UsedImplicitly]
	public sealed class MainViewController : UIViewController, IScreenFor<MainViewModel>
	{
		private readonly IVisibleRegionProvider _visibleRegionProvider;
		private readonly MapView _mapView;
		private readonly DestinationAddressTextField _destinationAddressTextField;
		private readonly PickupAddressTextField _pickupAddressTextField;
		private readonly MyLocationButton _myLocationButton;
		private readonly UIImageView _centerPinImageView;
		private readonly UIStackView _addresseStackView;
		private readonly ConfirmitionView _confirmitionView;
	    private readonly SetPickupLocationView _setPickupLocationView;

		private bool _hidden;
		private NSLayoutConstraint _confirmationViewHeightConstraint;
		private List<Binding> _bindings;

		public MainViewController(
			IVisibleRegionProvider visibleRegionProvider,
			IAutoMapperFacade autoMapperFacade)
		{
			_visibleRegionProvider = visibleRegionProvider;
			_mapView = new MapView();
			_myLocationButton = new MyLocationButton();
			_confirmitionView = new ConfirmitionView();

			_destinationAddressTextField = new DestinationAddressTextField
			{
				ShouldBeginEditing = DestinationAddressShouldBeginEditing
			};

			_pickupAddressTextField = new PickupAddressTextField
			{
				ShouldBeginEditing = PickupAddressShouldBeginEditing
			};

			_centerPinImageView = new UIImageView(Icons.PickupPin)
			{
				ContentMode = UIViewContentMode.ScaleAspectFit,
				TranslatesAutoresizingMaskIntoConstraints = false
			};

			_addresseStackView = new UIStackView(new UIView[] { _pickupAddressTextField, _destinationAddressTextField })
			{
				Axis = UILayoutConstraintAxis.Vertical,
				TranslatesAutoresizingMaskIntoConstraints = false,
				Distribution = UIStackViewDistribution.FillEqually,
				Spacing = 2
			};

            _setPickupLocationView = new SetPickupLocationView();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationController.NavigationBar.BarTintColor = UIColor.White;
			NavigationController.NavigationBar.Translucent = false;
			NavigationItem.Title = "Set pickup location";
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			AddCommands();
			SetupBindings();
			AddSubViews();
			SetupConstraints();
			AddSubscribers();
			await ViewModel.Load();
		    _setPickupLocationView.SetPickupButton.TouchUpInside += _setPickupButton_TouchUpInside;
		}

		private void _setPickupButton_TouchUpInside(object sender, EventArgs e)
		{
			_hidden = !_hidden;
			UIView.Animate(0.1, 0, UIViewAnimationOptions.CurveEaseIn, () =>
			{
				_confirmationViewHeightConstraint.Active = _hidden;
				//_setPickupButton.Hidden = true;
				View.SetNeedsLayout();
				View.LayoutIfNeeded();
			}, null);
		}

		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();

		    if (ViewModel.RideViewModel.HasPickupLocation)
		    {
		        _pickupAddressTextField.RoundCorners(UIRectCorner.TopLeft | UIRectCorner.TopRight, 5);
		        _destinationAddressTextField.RoundCorners(UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 5);
		        _destinationAddressTextField.Alpha = 1;
		    }
		    else
		    {
		        _pickupAddressTextField.RoundCorners(UIRectCorner.AllCorners, 5);
		        _destinationAddressTextField.Alpha = 0;
		    }
            _myLocationButton.ToCircleView();
		}

		private void AddSubscribers()
		{
			var cameraPositionIdle = Observable
				.FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
					e => _mapView.CameraPositionIdle += e,
					e => _mapView.CameraPositionIdle -= e);

			ViewModel.CameraPositionObservable = cameraPositionIdle
				.Select(e => e.EventArgs.Position.Target.ToLocation());

			ViewModel.VisibleRegionChanged = cameraPositionIdle
				.Select(_ => new CoordinateBounds(_mapView.Projection.VisibleRegion))
				.Select(bounds => _visibleRegionProvider.GetVisibleRegion(bounds.NorthEast.ToLocation(), bounds.SouthWest.ToLocation()));

			ViewModel.CameraStartMoving = Observable
				.FromEventPattern<EventHandler<GMSWillMoveEventArgs>, GMSWillMoveEventArgs>(
					e => _mapView.WillMove += e,
					e => _mapView.WillMove -= e)
				.Select(e => e.EventArgs.Gesture);

			ViewModel.CameraStartMoving
				.Subscribe(CameraWillMove);

			ViewModel.CameraPositionObservable
				.Subscribe(CamerPostionIdle);
		}

		private void AddCommands()
		{
			this.BindCommand(_setPickupLocationView.SetPickupButton, ViewModel.RideViewModel.SetPickupLocation);
			this.BindCommand(_myLocationButton, ViewModel.GoToMyLocation);
		}

		private void SetupBindings()
		{
			_bindings = new List<Binding>
			{
				this.SetBinding(
					() => _pickupAddressTextField.Text,
					() => ViewModel.RideViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay),

				this.SetBinding(
					() => _destinationAddressTextField.Text,
					() => ViewModel.RideViewModel.DestinationAddress.FormattedAddress, BindingMode.TwoWay),

				this.SetBinding(
					() => _pickupAddressTextField.Loading,
					() => ViewModel.RideViewModel.IsPickupAddressLoading, BindingMode.TwoWay),

				this.SetBinding(
					() => _mapView.OnlineDrivers,
					() => ViewModel.OnlineDrivers, BindingMode.TwoWay),

				this.SetBinding(
						() => _mapView.Camera,
						() => ViewModel.CameraTarget, BindingMode.TwoWay)
					.ConvertTargetToSource(location =>
						CameraPosition.FromCamera(location.Latitude,location.Longitude, 17)),

                this.SetBinding(
                    () => ViewModel.RideViewModel.HasPickupLocation)
                    .WhenSourceChanges(ViewDidLayoutSubviews)
			};
		}

		private void AddSubViews()
		{
			View.AddSubview(_mapView);
			View.AddSubview(_addresseStackView);
			View.AddSubview(_centerPinImageView);
            View.AddSubview(_setPickupLocationView);
			View.AddSubview(_myLocationButton);
			View.AddSubview(_confirmitionView);
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
				_myLocationButton.HeightAnchor.ConstraintEqualTo(40)
			});

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_centerPinImageView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
				NSLayoutConstraint.Create(_centerPinImageView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, View.SafeAreaLayoutGuide,
					NSLayoutAttribute.CenterY, (nfloat) 0.93, 0),
				_centerPinImageView.WidthAnchor.ConstraintEqualTo(40),
				_centerPinImageView.HeightAnchor.ConstraintEqualTo(40)
			});

		    NSLayoutConstraint.ActivateConstraints(new[]
		    {
		        _setPickupLocationView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
                NSLayoutConstraint.Create(_setPickupLocationView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, View.SafeAreaLayoutGuide,
                    NSLayoutAttribute.CenterY, (nfloat) 0.88, 0),
		        _setPickupLocationView.WidthAnchor.ConstraintEqualTo(305),
		        _setPickupLocationView.HeightAnchor.ConstraintEqualTo(40)
            });

            _confirmationViewHeightConstraint = _confirmitionView.HeightAnchor.ConstraintEqualTo(View.HeightAnchor, (nfloat)0.35);

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_confirmitionView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor),
				_confirmitionView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor),
			});
		}

		private void CamerPostionIdle(Location location)
		{
			var bounds = new CoordinateBounds(_mapView.Projection.VisibleRegion);

			//CLLocationCoordinate2D northEast = bounds.NorthEast;
			//CLLocationCoordinate2D northWest = new CLLocationCoordinate2D(bounds.NorthEast.Latitude, bounds.SouthWest.Longitude);
			//CLLocationCoordinate2D southEast = new CLLocationCoordinate2D(bounds.SouthWest.Latitude, bounds.NorthEast.Longitude);
			//CLLocationCoordinate2D southWest = bounds.SouthWest;

			UIView.Animate(0.3, () =>
			{
				_myLocationButton.Hidden = false;
			    _setPickupLocationView.Transform = CGAffineTransform.MakeIdentity();
			    _setPickupLocationView.Alpha = 1;
			});
		}

		private void CameraWillMove(bool gesture)
		{
			UIView.Animate(0.3, () =>
			{
				_myLocationButton.Hidden = true;
			    _setPickupLocationView.Transform = CGAffineTransform.MakeScale((nfloat)0.1, 1);
			    _setPickupLocationView.Alpha = 0;
			});
		}

		private bool DestinationAddressShouldBeginEditing(UITextField textField)
		{
			ViewModel.NavigateToDestinationSearch.Execute(null);
			return false;
		}

		private bool PickupAddressShouldBeginEditing(UITextField textField)
		{
			ViewModel.NavigateToPickupSearch.Execute(null);
			return false;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				ViewModel.Dispose();
				_bindings.ForEach(b => b.Detach());
			}
		}

		public MainViewModel ViewModel { get; set; }
	}
}