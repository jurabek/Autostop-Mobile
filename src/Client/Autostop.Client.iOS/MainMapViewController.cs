using System;
using System.Collections.Generic;
using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Core;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Extensions;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using JetBrains.Annotations;
using UIKit;

namespace Autostop.Client.iOS
{
    public partial class MainMapViewController : UIViewController, IScreenFor<MainViewModel>
    {
        private readonly IContainer _container = BootstrapperBase.Container;
        [UsedImplicitly] private List<Binding> _bindings;
        [UsedImplicitly] private List<IDisposable> _subscribers;
        private UIActivityIndicatorView _locationLoadActivatyIndacator;
        private UIImageView _destinationLocationLeftImageView;
        private UIImageView _pickupLocationLeftImageView;

        [UsedImplicitly]
        public MainViewModel ViewModel { get; set; }

        public MainMapViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            MainMapView.MyLocationEnabled = true;
            ViewModel = _container.Resolve<MainViewModel>();
            SetPickupLocationButton
                .SetCommand(nameof(SetPickupLocationButton.TouchUpInside), ViewModel.SetPickupLocation);

            _bindings = new List<Binding>
            {
                this.SetBinding(
                    () => pickupLocationTextField.Text,
                    () => ViewModel.PickupLocation.FormattedAddress, BindingMode.TwoWay)
            };

            _subscribers = new List<IDisposable>
            {
                ViewModel.ObservablePropertyChanged<bool>(nameof(MainViewModel.HasPickupLocation))
                    .Subscribe(hasPickupLocation =>
                    {
                        if (hasPickupLocation)
                            SetPickupLocation();
                        else
                            InitLocationTextFields();
                    }),

                ViewModel.ObservablePropertyChanged(() => ViewModel.IsPickupLocationLoading)
                    .Subscribe(loading =>
                    {
                        if (loading)
                        {
                            _locationLoadActivatyIndacator.StartAnimating();
                            pickupLocationTextField.LeftView = _locationLoadActivatyIndacator;
                        }
                        else
                        {
                            _locationLoadActivatyIndacator.StopAnimating();
                            SetLeftIconToPickupLocationTextField();
                        }
                    }),

                ViewModel.CurrentLocation.Subscribe(l =>
                    {
                        var camera = CameraPosition.FromCamera(l.Coordinate.Latitude, l.Coordinate.Longitude, 17);
                        MainMapView.Camera = camera;
                    })
            };

            InitViews();
        }

        private void InitViews()
        {
            _locationLoadActivatyIndacator = GetLocationsLoadActivityIndacator();
            _pickupLocationLeftImageView = GetLocationsTextFieldLeftImageView("pickup_location_dot.png");
            _destinationLocationLeftImageView = GetLocationsTextFieldLeftImageView("pickup_destination_dot.png");


            SetLeftIconToPickupLocationTextField();
            SetLeftIconToDestinationTextField();
            InitLocationTextFields();
        }

        private void InitLocationTextFields()
        {
            pickupLocationTextField.LeftViewMode = UITextFieldViewMode.Always;
            pickupLocationTextField.ShouldBeginEditing = _ => false;
            pickupLocationTextField.RoundCorners(UIRectCorner.AllCorners, 8);

            destinationTextField.LeftViewMode = UITextFieldViewMode.Always;
            destinationTextField.ShouldBeginEditing = _ => false;
            destinationTextField.Hidden = true;
        }

        private void SetPickupLocation()
        {
            destinationTextField.Hidden = false;
            pickupLocationTextField.RoundCorners(UIRectCorner.TopLeft | UIRectCorner.TopRight, 8);
            destinationTextField.RoundCorners(UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 8);
        }

        private void SetLeftIconToPickupLocationTextField()
        {
            pickupLocationTextField.LeftView = _pickupLocationLeftImageView;
        }

        private void SetLeftIconToDestinationTextField()
        {
            destinationTextField.LeftView = _destinationLocationLeftImageView;
        }

        private UIActivityIndicatorView GetLocationsLoadActivityIndacator()
        {
            var activityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray) { ContentMode = UIViewContentMode.Center };
            var size = activityIndicator.Frame.Size;
            activityIndicator.Frame = new CGRect(0, 0, size.Width + 15, size.Height);
            return activityIndicator;
        }

        private UIImageView GetLocationsTextFieldLeftImageView(string image)
        {
            var icon = new UIImageView(UIImage.FromFile(image));
            var size = icon.Image.Size;
            icon.ContentMode = UIViewContentMode.Center;
            icon.Frame = new CGRect(0, 0, size.Width + 10, size.Height);
            return icon;
        }
    }
}