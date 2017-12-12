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
using UIKit;

namespace Autostop.Client.iOS
{
	public partial class MainMapViewController : UIViewController
	{
		private readonly IContainer _container = BootstrapperBase.Container;
		private readonly List<Binding> _bindings = new List<Binding>();
		private UIActivityIndicatorView _activatyIndacator;

		public MainViewModel ViewModel { get; set; }

		public MainMapViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ViewModel = _container.Resolve<MainViewModel>();

			ViewModel.Changed.Where(p => p.PropertyName == nameof(MainViewModel.HasPickupLocation))
				.Select(_ => ViewModel.HasPickupLocation)
				.Subscribe(hasPickupLocation =>
				{
					if (hasPickupLocation)
						SetPickupLocation();
					else
						InitPickupLocationView();
				});
				
			var camera = CameraPosition.FromCamera(37.797865, -122.402526, 14);
			MainMapView.Camera = camera;

			MainMapView.WillMove += MainMapView_WillMove;
			MainMapView.CameraPositionChanged += MainMapView_CameraPositionChanged;
			MainMapView.CameraPositionIdle += MainMapView_CameraPositionIdle;
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

		private void MainMapView_CameraPositionIdle(object sender, GMSCameraEventArgs e)
		{
			Console.WriteLine($"Idle: {e.Position.Target}");
		}

		private void MainMapView_CameraPositionChanged(object sender, GMSCameraEventArgs e)
		{
			Console.WriteLine($"Changed: {e.Position.Target}");
		}

		private void MainMapView_WillMove(object sender, GMSWillMoveEventArgs e)
		{
			Console.WriteLine(e.Gesture);
			pickupLocationTextField.LeftView = _activatyIndacator;
		}
	}
}