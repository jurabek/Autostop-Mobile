using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Facades;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using Autostop.Client.iOS.UI;
using CoreGraphics;
using CoreLocation;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using JetBrains.Annotations;
using UIKit;
using MapView = Autostop.Client.iOS.UI.MapView;
using Location = Autostop.Common.Shared.Models.Location;

namespace Autostop.Client.iOS.Views.Passenger
{
    [UsedImplicitly]
    public class MainViewController : UIViewController, IScreenFor<MainViewModel>
    {
        private readonly MapView _mapView;
        private readonly DestinationAddressTextField _destinationAddressTextField;
        private readonly PickupAddressTextField _pickupAddressTextField;
        private readonly MyLocationButton _myLocationButton;
        private readonly UIImageView _centerPinImageView;
        private readonly UIButton _setPickupButton;
        private readonly UIStackView _addresseStackView;
        private readonly ConfirmitionView _confirmitionView;

        private bool _hidden;
        private NSLayoutConstraint _confirmationViewHeightConstraint;
        private List<Binding> _bindings;

        public MainViewController(IAutoMapperFacade autoMapperFacade)
        {
            _mapView = new MapView();
            _myLocationButton = new MyLocationButton();

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

            _setPickupButton = new UIButton
            {
                BackgroundColor = Colors.PickupButtonColor,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _confirmitionView = new ConfirmitionView
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _addresseStackView = new UIStackView(new UIView[] { _pickupAddressTextField, _destinationAddressTextField })
            {
                Axis = UILayoutConstraintAxis.Vertical,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Distribution = UIStackViewDistribution.FillEqually,
                Spacing = 2
            };

            _setPickupButton.Layer.CornerRadius = 20;
            _setPickupButton.SetTitle("SET PICKUP LOCATION", UIControlState.Normal);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.BarTintColor = UIColor.White;
            NavigationController.NavigationBar.Translucent = false;
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
            _setPickupButton.TouchUpInside += _setPickupButton_TouchUpInside;
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

            _myLocationButton.ToCircleButton();
        }

        private void AddSubscribers()
        {
            ViewModel.CameraPositionObservable = Observable
                .FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
                    e => _mapView.CameraPositionIdle += e,
                    e => _mapView.CameraPositionIdle -= e)
                .Select(e => e.EventArgs.Position.Target)
                .Select(c => new Location(c.Latitude, c.Longitude));

            ViewModel.CameraStartMoving = Observable
                .FromEventPattern<EventHandler<GMSWillMoveEventArgs>, GMSWillMoveEventArgs>(
                    e => _mapView.WillMove += e,
                    e => _mapView.WillMove -= e)
                .Select(e => e.EventArgs.Gesture);

            ViewModel.RideViewModel.ObservablePropertyChanged(() => ViewModel.RideViewModel.HasPickupLocation)
                .Subscribe(_ => ViewDidLayoutSubviews());

            ViewModel.CameraStartMoving
                .Subscribe(CameraWillMove);

            ViewModel.CameraPositionObservable
                .Subscribe(CamerPostionIdle);
        }

        private void AddCommands()
        {
            this.BindCommand(_setPickupButton, ViewModel.RideViewModel.SetPickupLocation);
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
                        CameraPosition.FromCamera(location.Latitude,location.Longitude, 17))
            };
        }

        private void AddSubViews()
        {
            View.AddSubview(_mapView);
            View.AddSubview(_addresseStackView);
            View.AddSubview(_centerPinImageView);
            View.AddSubview(_setPickupButton);
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
                _centerPinImageView.WidthAnchor.ConstraintEqualTo(46),
                _centerPinImageView.HeightAnchor.ConstraintEqualTo(60)
            });

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _setPickupButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
                NSLayoutConstraint.Create(_setPickupButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, View.SafeAreaLayoutGuide,
                    NSLayoutAttribute.CenterY, (nfloat) 0.88, 0),
                _setPickupButton.WidthAnchor.ConstraintEqualTo(305),
                _setPickupButton.HeightAnchor.ConstraintEqualTo(35)
            });

            _confirmationViewHeightConstraint = _confirmitionView.HeightAnchor.ConstraintEqualTo(View.HeightAnchor, (nfloat)0.35);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _confirmitionView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor),
                _confirmitionView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor),
            });
        }

        private void CamerPostionIdle(Common.Shared.Models.Location location)
        {
            UIView.Animate(0.3, () =>
            {
                _myLocationButton.Hidden = false;
                _setPickupButton.Transform = CGAffineTransform.MakeIdentity();
                _setPickupButton.Alpha = 1;
            });
        }

        private void CameraWillMove(bool gesture)
        {
            UIView.Animate(0.3, () =>
            {
                _myLocationButton.Hidden = true;
                _setPickupButton.Transform = CGAffineTransform.MakeScale((nfloat)0.1, 1);
                _setPickupButton.Alpha = 0;
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