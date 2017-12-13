using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Autofac;
using Autostop.Client.Core;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Extensions;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using JetBrains.Annotations;
using UIKit;

namespace Autostop.Client.iOS
{
	public partial class MainMapViewController : UIViewController
	{
		private readonly IContainer _container = BootstrapperBase.Container;
		private readonly List<Binding> _bindings = new List<Binding>();
		private UIActivityIndicatorView _activatyIndacator;
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

			ViewModel = _container.Resolve<MainViewModel>();

			_bindings.Add(this.SetBinding(
					() => pickupLocationTextField.Text, 
					() => ViewModel.PickupLocation.FormattedAddress, BindingMode.TwoWay));

			SetPickupLocationButton.SetCommand(nameof(SetPickupLocationButton.TouchUpInside), ViewModel.SetPickupLocation);

            ViewModel.Changed.Where(p => p.PropertyName == nameof(MainViewModel.HasPickupLocation))
				.Select(_ => ViewModel.HasPickupLocation)
				.Subscribe(hasPickupLocation =>
				{
					if (hasPickupLocation)
						SetPickupLocation();
					else
						InitLocationTextFields();
				});

			ViewModel.Changed.Where(p => p.PropertyName == nameof(MainViewModel.IsPickupLocationLoading))
				.Select(_ => ViewModel.IsPickupLocationLoading)
				.Subscribe(loading =>
				{
					if (loading)
					{
						_activatyIndacator.StartAnimating();
						pickupLocationTextField.LeftView = _activatyIndacator;
					}
					else
					{
						_activatyIndacator.StopAnimating();
						pickupLocationTextField.LeftView = _pickupLocationLeftImageView;
					}
				});

		    ViewModel.CurrentLocation.Subscribe(l =>
		        {
		            var camera = CameraPosition.FromCamera(l.Coordinate.Latitude, l.Coordinate.Longitude, 17);
		            MainMapView.Camera = camera;
                });
			
		    MainMapView.MyLocationEnabled = true;
			InitViews();
		}

		private void InitViews()
		{
			_activatyIndacator = GetActivityIndacator();
			_pickupLocationLeftImageView = GetPickupLocationLeftImageView();
			_destinationLocationLeftImageView = GetDestinationLocationLeftImageView();
			

			SetLeftIconToPickupLocationTextField();
			SetLeftIconToDestinationTextField();
			InitLocationTextFields();
		}

		private void InitLocationTextFields()
		{
			pickupLocationTextField.LeftViewMode = UITextFieldViewMode.Always;
			pickupLocationTextField.RoundCorners(UIRectCorner.AllCorners, 8);
			pickupLocationTextField.ShouldBeginEditing = f => false;

			destinationTextField.LeftViewMode = UITextFieldViewMode.Always;
			destinationTextField.Hidden = true;
			destinationTextField.ShouldBeginEditing = field => false; 
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

		private UIActivityIndicatorView GetActivityIndacator()
		{
			var activityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray) { ContentMode = UIViewContentMode.Center };
			var size = activityIndicator.Frame.Size;
			activityIndicator.Frame = new CGRect(0, 0, size.Width + 15, size.Height);
			return activityIndicator;
		}

		private UIImageView GetDestinationLocationLeftImageView()
		{
			var icon = new UIImageView(UIImage.FromFile("pickup_destination_dot.png"));
			var size = icon.Image.Size;
			icon.ContentMode = UIViewContentMode.Center;
			icon.Frame = new CGRect(0, 0, size.Width + 10, size.Height);

			return icon;
		}

		private UIImageView GetPickupLocationLeftImageView()
		{
			var icon = new UIImageView(UIImage.FromFile("pickup_location_dot.png"));
			var size = icon.Image.Size;
			icon.ContentMode = UIViewContentMode.Center;
			icon.Frame = new CGRect(0, 0, size.Width + 10, size.Height);

			return icon;
		}
	}
}