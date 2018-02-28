using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Facades;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using Autostop.Client.iOS.UI;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Shared.UI;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Location = Autostop.Common.Shared.Models.Location;
using MapView = Autostop.Client.iOS.UI.MapView;

namespace Autostop.Client.iOS.Views.Passenger
{
	public sealed class MainViewController : UIViewController, IScreenFor<MainViewModel>
	{
		private readonly IVisibleRegionProvider _visibleRegionProvider;
		private readonly MapView _mapView;
		private readonly UIButton _whereToGoButton;
		private readonly PickupAddressTextField _pickupAddressTextField;
		private readonly MyLocationButton _myLocationButton;
		private readonly UIImageView _centerPinImageView;
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

			_whereToGoButton = new UIButton
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = AutostopColors.White.ToUIColor(),
				TintColor = UIColor.Black
			};


			_pickupAddressTextField = new PickupAddressTextField
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				ShouldBeginEditing = PickupAddressShouldBeginEditing,
				BackgroundColor = UIColor.FromRGBA(255, 255, 255, 200)
			};

			_centerPinImageView = new UIImageView(Icons.PickupPin)
			{
				ContentMode = UIViewContentMode.ScaleAspectFit,
				TranslatesAutoresizingMaskIntoConstraints = false
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
			_whereToGoButton.SetTitle("Where to go?", UIControlState.Normal);

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
				View.SetNeedsLayout();
				View.LayoutIfNeeded();
			}, null);
		}

		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();

		    //if (ViewModel.TripLocationViewModel.HasPickupLocation)
		    //{
		    //    //_pickupAddressTextField.RoundCorners(UIRectCorner.TopLeft | UIRectCorner.TopRight, 5);
		    //    //_whereToGoButton.RoundCorners(UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 5);
		    //    //_whereToGoButton.Alpha = 1;
		    //}
		    //else
		    //{
		    //    //_pickupAddressTextField.RoundCorners(UIRectCorner.AllCorners, 5);
		    //    //_whereToGoButton.Alpha = 0;
		    //}

			_whereToGoButton.Layer.CornerRadius = 20;
			_myLocationButton.ToCircleView();
		}

		private void AddSubscribers()
		{
			var cameraPositionIdle = Observable
				.FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
					e => _mapView.CameraPositionIdle += e,
					e => _mapView.CameraPositionIdle -= e);

			ViewModel.CameraPositionChanged = cameraPositionIdle
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

			ViewModel.CameraPositionChanged
				.Subscribe(CamerPostionIdle);
		}

		private void AddCommands()
		{
			this.BindCommand(_myLocationButton, ViewModel.GoToMyLocation);
			this.BindCommand(_whereToGoButton, ViewModel.TripLocationViewModel.NavigateToWhereTo);
		}

		private void SetupBindings()
		{
			_bindings = new List<Binding>
			{
				this.SetBinding(
					() => _pickupAddressTextField.Text,
					() => ViewModel.TripLocationViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay),

				this.SetBinding(
					() => _pickupAddressTextField.Loading,
					() => ViewModel.TripLocationViewModel.PickupAddress.Loading, BindingMode.TwoWay),

				this.SetBinding(
					() => _mapView.OnlineDrivers,
					() => ViewModel.OnlineDrivers, BindingMode.TwoWay),

				this.SetBinding(
						() => _mapView.Camera,
						() => ViewModel.CameraTarget, BindingMode.TwoWay)
					.ConvertTargetToSource(location =>
						CameraPosition.FromCamera(location.Latitude,location.Longitude, 17)),

                //this.SetBinding(
                //    () => ViewModel.TripLocationViewModel.HasPickupLocation)
                //    .WhenSourceChanges(ViewDidLayoutSubviews)
			};
		}

		private void AddSubViews()
		{
			View.AddSubview(_mapView);
			View.AddSubview(_pickupAddressTextField);
			View.AddSubview(_whereToGoButton);
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
				_pickupAddressTextField.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor, 6),
				_pickupAddressTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 10),
				_pickupAddressTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -10),
				_pickupAddressTextField.HeightAnchor.ConstraintEqualTo(40)
			});

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_whereToGoButton.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor, -30),
				_whereToGoButton.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 25),
				_whereToGoButton.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -25),
				_whereToGoButton.HeightAnchor.ConstraintEqualTo(40)
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
				_centerPinImageView.WidthAnchor.ConstraintEqualTo(50),
				_centerPinImageView.HeightAnchor.ConstraintEqualTo(50)
			});

		    NSLayoutConstraint.ActivateConstraints(new[]
		    {
		        _setPickupLocationView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
                NSLayoutConstraint.Create(_setPickupLocationView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, View.SafeAreaLayoutGuide,
                    NSLayoutAttribute.CenterY, (nfloat) 0.89, 0),
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

		private bool PickupAddressShouldBeginEditing(UITextField textField)
		{
			ViewModel.TripLocationViewModel.NavigateToPickupSearch.Execute(null);
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