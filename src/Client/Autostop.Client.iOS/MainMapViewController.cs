using System;
using System.Linq;
using Autostop.Client.iOS.Extensions;
using CoreAnimation;
using CoreGraphics;
using CoreLocation;
using Google.Maps;
using UIKit;

namespace Autostop.Client.iOS
{
	public partial class MainMapViewController : UIViewController
	{
		
		public MainMapViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			CameraPosition camera = CameraPosition.FromCamera(37.797865, -122.402526, 14);
			MainMapView.Camera = camera;

			pickupLocationTextField.LeftViewMode = UITextFieldViewMode.Always;
			SetLeftIconToPickupLocationTextField();

			destinationTextField.LeftViewMode = UITextFieldViewMode.Always;
			SetLeftIconToDestinationTextField();

			pickupLocationTextField.RoundCorners(UIRectCorner.TopLeft | UIRectCorner.TopRight, 8);
			destinationTextField.RoundCorners(UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 8);

			MainMapView.WillMove += MainMapView_WillMove;
			MainMapView.CameraPositionChanged += MainMapView_CameraPositionChanged;
			MainMapView.CameraPositionIdle += MainMapView_CameraPositionIdle;
		}

		private void SetLeftIconToPickupLocationTextField()
		{
			var icon = new UIImageView(UIImage.FromFile("pickup_location_dot.png"));;
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
			var activateIndacator =new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray)
			{
				ContentMode = UIViewContentMode.Center
			};
			var size = activateIndacator.Frame.Size;
			activateIndacator.Frame = new CGRect(0, 0, size.Width + 10, size.Height);

			activateIndacator.StartAnimating();
			pickupLocationTextField.LeftView = activateIndacator;
		}
	}
}