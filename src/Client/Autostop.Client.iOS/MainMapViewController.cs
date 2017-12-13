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

	    [UsedImplicitly]
        public MainViewModel ViewModel { get; set; }

		public MainMapViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ViewModel = _container.Resolve<MainViewModel>();
		    SetPickupLocationButton.SetCommand(nameof(SetPickupLocationButton.TouchUpInside), ViewModel.SetPickupLocation);

            ViewModel.Changed.Where(p => p.PropertyName == nameof(MainViewModel.HasPickupLocation))
				.Select(_ => ViewModel.HasPickupLocation)
				.Subscribe(hasPickupLocation =>
				{
					if (hasPickupLocation)
						SetPickupLocation();
					else
						InitPickupLocationView();
				});

		    ViewModel.CurrentLocation.Subscribe(l =>
		        {
		            var camera = CameraPosition.FromCamera(l.Coordinate.Latitude, l.Coordinate.Longitude, 14);
		            MainMapView.Camera = camera;
                });
			
		    MainMapView.MyLocationEnabled = true;
			InitViews();
		}

		private void InitViews()
		{
			_activatyIndacator = GetActivityIndacator();
			pickupLocationTextField.LeftViewMode = UITextFieldViewMode.Always;
			destinationTextField.LeftViewMode = UITextFieldViewMode.Always;

			SetLeftIconToPickupLocationTextField();
			SetLeftIconToDestinationTextField();
			InitPickupLocationView();
		}

		private void InitPickupLocationView()
		{
			destinationTextField.Hidden = true;
			pickupLocationTextField.RoundCorners(UIRectCorner.AllCorners, 8);
		}

		private void SetPickupLocation()
		{
			destinationTextField.Hidden = false;
			pickupLocationTextField.RoundCorners(UIRectCorner.TopLeft | UIRectCorner.TopRight, 8);
			destinationTextField.RoundCorners(UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 8);
		}

		private void SetLeftIconToPickupLocationTextField()
		{
			var icon = new UIImageView(UIImage.FromFile("pickup_location_dot.png"));
			var size = icon.Image.Size;
			icon.ContentMode = UIViewContentMode.Center;
			icon.Frame = new CGRect(0, 0, size.Width + 10, size.Height);
			pickupLocationTextField.LeftView = icon;
		}

		private void SetLeftIconToDestinationTextField()
		{
			var icon = new UIImageView(UIImage.FromFile("pickup_destination_dot.png"));
			var size = icon.Image.Size;
			icon.ContentMode = UIViewContentMode.Center;
			icon.Frame = new CGRect(0, 0, size.Width + 10, size.Height);
			destinationTextField.LeftView = icon;
		}

		private UIActivityIndicatorView GetActivityIndacator()
		{
			var activityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray) { ContentMode = UIViewContentMode.Center };
			var size = activityIndicator.Frame.Size;
			activityIndicator.Frame = new CGRect(0, 0, size.Width + 15, size.Height);
			return activityIndicator;
		}
	}
}