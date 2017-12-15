using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Core;
using Autostop.Client.Core.Enums;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Extensions;
using Autostop.Common.Shared.Models;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using JetBrains.Annotations;
using UIKit;

namespace Autostop.Client.iOS
{
    public partial class MainViewController : UIViewController
    {

        private readonly IContainer _container = BootstrapperBase.Container;
        [UsedImplicitly] private List<Binding> _bindings;
        [UsedImplicitly] private List<IDisposable> _subscribers;
        private UIActivityIndicatorView _locationLoadActivatyIndacator;
        private UIImageView _destinationLocationLeftImageView;
        private UIImageView _pickupLocationLeftImageView;

        [UsedImplicitly]
        public MainViewModel ViewModel { get; set; }

        public MainViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel = _container.Resolve<MainViewModel>();

            _bindings = new List<Binding>
            {
                this.SetBinding(
                    () => pickupAddressTextField.Text,
                    () => ViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay),

                this.SetBinding(
                    () => destinationAddressTextField.Text,
                    () => ViewModel.DestinationAddress.FormattedAddress, BindingMode.TwoWay),

                this.SetBinding(
                    () => destinationAddressTextField.Hidden,
                    () => ViewModel.AddressMode)
                    .ConvertTargetToSource(mode => mode == AddressMode.Destination)
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

                ViewModel.ObservablePropertyChanged(() => ViewModel.IsPickupAddressLoading)
                    .Subscribe(loading =>
                    {
                        if (loading)
                        {
                            _locationLoadActivatyIndacator.StartAnimating();
                            pickupAddressTextField.LeftView = _locationLoadActivatyIndacator;
                        }
                        else
                        {
                            _locationLoadActivatyIndacator.StopAnimating();
                            SetLeftIconToPickupLocationTextField();
                        }
                    }),

                ViewModel.CurrentLocationObservable.Subscribe(l =>
                    {
                        var camera = CameraPosition.FromCamera(l.Latitude, l.Longitude, 17);
                        gmsMapView.Camera = camera;
                    })
            };

            ViewModel.CameraLocationObservable = Observable.FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
                 e => gmsMapView.CameraPositionIdle += e,
                 e => gmsMapView.CameraPositionIdle -= e)
                 .Select(e => e.EventArgs.Position.Target)
                 .Select(c => new Location(c.Latitude, c.Longitude));


            setPickupLocationButton.SetCommand("TouchUpInside", ViewModel.SetPickupLocation);
            gmsMapView.MyLocationEnabled = true;
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
            pickupAddressTextField.LeftViewMode = UITextFieldViewMode.Always;
            pickupAddressTextField.ShouldBeginEditing = _ => false;
            pickupAddressTextField.RoundCorners(UIRectCorner.AllCorners, 8);

            destinationAddressTextField.LeftViewMode = UITextFieldViewMode.Always;
            destinationAddressTextField.ShouldBeginEditing = _ => false;
        }

        private void SetPickupLocation()
        {
            pickupAddressTextField.RoundCorners(UIRectCorner.TopLeft | UIRectCorner.TopRight, 8);
            destinationAddressTextField.RoundCorners(UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 8);
        }

        private void SetLeftIconToPickupLocationTextField()
        {
            pickupAddressTextField.LeftView = _pickupLocationLeftImageView;
        }

        private void SetLeftIconToDestinationTextField()
        {
            destinationAddressTextField.LeftView = _destinationLocationLeftImageView;
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